package tech.mercantec.storagesystem

import android.app.Activity
import android.app.AlertDialog
import android.app.ProgressDialog
import android.content.Intent
import android.util.Log
import android.widget.Toast
import androidx.annotation.OptIn
import androidx.camera.core.ExperimentalGetImage
import androidx.camera.core.ImageAnalysis
import androidx.camera.core.ImageProxy
import com.google.mlkit.common.MlKitException
import com.google.mlkit.vision.barcode.BarcodeScanning
import com.google.mlkit.vision.common.InputImage
import kotlinx.serialization.Serializable
import tech.mercantec.storagesystem.services.Api
import tech.mercantec.storagesystem.services.ApiRequestException
import tech.mercantec.storagesystem.ui.CreateProductActivity
import tech.mercantec.storagesystem.ui.ViewProductActivity
import java.util.concurrent.Executors
import kotlin.concurrent.thread

class ImageAnalyzer(val analysis: ImageAnalysis, val ctx: Activity) : ImageAnalysis.Analyzer {
    val api = Api(ctx)

    @OptIn(ExperimentalGetImage::class)
    override fun analyze(imageProxy: ImageProxy) {
        if (imageProxy.image == null) return
        val image = InputImage.fromMediaImage(imageProxy.image!!, imageProxy.imageInfo.rotationDegrees)

        val scanner = BarcodeScanning.getClient()

        scanner.process(image)
            .addOnSuccessListener { barcodes ->
                if (barcodes.isEmpty()) return@addOnSuccessListener

                val barcode = barcodes[0].rawValue
                if (barcode != null) processBarcode(barcode)

                analysis.clearAnalyzer()
            }
            .addOnFailureListener { e ->
                Log.e("StorageSystem", e.toString() + " (error code ${(e as MlKitException).errorCode})")
            }
            .addOnCompleteListener { imageProxy.close() }
    }

    private fun processBarcode(barcode: String) {
        @Serializable
        data class BarcodeScanResponseBaseItemSpecificItem(val id: Int, val description: String)

        @Serializable
        data class BarcodeScanResponseBaseItem(val id: Int, val name: String, val description: String, val image_url: String?, val specific_items: ArrayList<BarcodeScanResponseBaseItemSpecificItem>)

        @Serializable
        data class BarcodeScanResponseSpecificItem(val id: Int, val name: String, val description: String, val image_url: String?, val base_item: BarcodeScanResponseBaseItem)

        @Serializable
        data class BarcodeScanResponseShelfBaseItem(val id: Int, val name: String, val description: String, val image_url: String?)

        @Serializable
        data class BarcodeScanResponseShelf(val id: Int, val shelf_no: Int, val rack_id: Int, val base_items: ArrayList<BarcodeScanResponseShelfBaseItem>)

        @Serializable
        data class BarcodeScanResponse(val type: String, val base_item: BarcodeScanResponseBaseItem?, val specific_item: BarcodeScanResponseSpecificItem?, val shelf: BarcodeScanResponseShelf?)

        val progressDialog = ProgressDialog(ctx).apply {
            setMessage("Loading...")
            show()
        }

        thread {
            try {
                val response = api.requestJson<Unit, BarcodeScanResponse>("POST", "/barcodes/scan?barcode=${barcode}", null)

                ctx.runOnUiThread {
                    when (response.type) {
                        "base_item" -> {
                            val intent = Intent(ctx, ViewProductActivity::class.java)
                            intent.putExtra("baseItemId", response.base_item!!.id)
                            ctx.startActivity(intent)
                        }
                        "specific_item" -> {
                            val intent = Intent(ctx, ViewProductActivity::class.java)
                            intent.putExtra("baseItemId", response.specific_item!!.base_item.id)
                            ctx.startActivity(intent)
                        }
                        "shelf" -> {
                            Toast.makeText(ctx, "Found shelf: ${response.shelf!!.shelf_no}", Toast.LENGTH_LONG).show()
                        }
                        else -> Toast.makeText(ctx, "Invalid response type: ${response.type}", Toast.LENGTH_LONG).show()
                    }

                    analysis.setAnalyzer(Executors.newSingleThreadExecutor(), this)
                }
            } catch (e: ApiRequestException) {
                if (e.code == 404) {
                    // Barcode not found, prompt product creation
                    ctx.runOnUiThread {
                        val dialog = AlertDialog.Builder(ctx)
                            .setMessage("Barcode doesn't exist. Create a new product?")
                            .setPositiveButton("Create") { dialog, which ->
                                val intent = Intent(ctx, CreateProductActivity::class.java)
                                intent.putExtra("barcode", barcode)
                                ctx.startActivity(intent)
                            }
                            .setNegativeButton("Cancel") { dialog, which -> }
                            .setOnDismissListener {
                                analysis.setAnalyzer(Executors.newSingleThreadExecutor(), this)
                            }
                            .create()

                        dialog.show()
                    }

                    return@thread
                }

                ctx.runOnUiThread {
                    Toast.makeText(ctx, e.message, Toast.LENGTH_LONG).show()
                }
            } finally {
                ctx.runOnUiThread { progressDialog.hide() }
            }
        }
    }
}
