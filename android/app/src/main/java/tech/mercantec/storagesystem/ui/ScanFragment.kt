package tech.mercantec.storagesystem.ui

import android.Manifest
import android.content.pm.PackageManager
import android.content.pm.PackageManager.PERMISSION_GRANTED
import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.annotation.OptIn
import androidx.camera.core.CameraSelector
import androidx.camera.core.ExperimentalGetImage
import androidx.camera.core.ImageAnalysis
import androidx.camera.core.ImageProxy
import androidx.camera.core.Preview
import androidx.camera.lifecycle.ProcessCameraProvider
import androidx.camera.view.LifecycleCameraController
import androidx.camera.view.PreviewView
import androidx.core.content.ContextCompat
import androidx.core.content.ContextCompat.checkSelfPermission
import com.google.mlkit.common.MlKitException
import com.google.mlkit.vision.barcode.BarcodeScanning
import com.google.mlkit.vision.common.InputImage
import tech.mercantec.storagesystem.R
import java.util.concurrent.Executors

class ScanFragment : Fragment() {
    private val CAMERA_PERMISSION_REQUEST_CODE = 200
    private lateinit var previewView: PreviewView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val cameraPermission = checkSelfPermission(requireContext(), Manifest.permission.CAMERA)

        if (cameraPermission == PERMISSION_GRANTED) {
            showCamera()
        } else {
            requireActivity().requestPermissions(arrayOf(Manifest.permission.CAMERA), CAMERA_PERMISSION_REQUEST_CODE)
        }
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String?>,
        grantResults: IntArray
    ) {
        if (requestCode == CAMERA_PERMISSION_REQUEST_CODE) {
            if (grantResults.isNotEmpty() && grantResults.contains(PERMISSION_GRANTED)) {
                showCamera()
            } else {
                // TODO handle rejection
            }

            return
        }

        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
    }

    @OptIn(ExperimentalGetImage::class)
    private fun showCamera() {
        val cameraProviderFuture = ProcessCameraProvider.getInstance(requireContext())

        cameraProviderFuture.addListener({
            val cameraProvider = cameraProviderFuture.get()

            val preview = Preview.Builder().build().apply { surfaceProvider = previewView.surfaceProvider }

            val scanner = BarcodeScanning.getClient()

            val imageAnalyzer = ImageAnalysis.Builder().build().also {
                it.setAnalyzer(Executors.newSingleThreadExecutor()) { imageProxy ->
                    Log.d("StorageSystem", "Analyzing image")
                    val image = imageProxy.image?.run { InputImage.fromMediaImage(this, imageProxy.imageInfo.rotationDegrees) }
                        ?: InputImage.fromBitmap(imageProxy.toBitmap(), imageProxy.imageInfo.rotationDegrees)

                    scanner.process(image)
                        .addOnSuccessListener { barcodes ->
                            if (barcodes.isEmpty()) return@addOnSuccessListener

                            val barcode = barcodes[0]

                            requireActivity().runOnUiThread {
                                Toast.makeText(requireContext(), barcode.rawValue, Toast.LENGTH_LONG).show()
                                it.clearAnalyzer()
                            }
                        }
                        .addOnFailureListener { e ->
                            Log.e("StorageSystem", e.toString() + " (error code ${(e as MlKitException).errorCode})")
                        }
                        .addOnCompleteListener { imageProxy.close() }
                }
            }

            cameraProvider.unbindAll()
            cameraProvider.bindToLifecycle(this, CameraSelector.DEFAULT_BACK_CAMERA, preview, imageAnalyzer)
        }, ContextCompat.getMainExecutor(requireContext()))
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_scan, container, false)

        previewView = view.findViewById(R.id.preview)

        return view
    }
}
