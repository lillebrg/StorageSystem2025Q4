package tech.mercantec.storagesystem.ui

import android.content.ActivityNotFoundException
import android.content.Intent
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.os.Bundle
import android.provider.MediaStore
import android.widget.*
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import kotlinx.serialization.Serializable
import kotlinx.serialization.json.Json
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.services.Api
import java.io.ByteArrayOutputStream

class CreateProductActivity : AppCompatActivity() {
    val api = Api(this)

    val PICK_IMAGE_REQUEST = 1
    val TAKE_PICTURE_REQUEST = 2

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_create_product)

        // Get product data from intent
        val id = intent.extras?.getInt("id")
        val name = intent.extras?.getString("name") ?: ""
        val description = intent.extras?.getString("description") ?: ""
        val barcode = intent.extras?.getString("barcode") ?: ""

        // Change text on page if updating instead of creating
        if (id != 0) {
            findViewById<TextView>(R.id.title).setText(R.string.update_product, TextView.BufferType.SPANNABLE)
            findViewById<TextView>(R.id.save_button).setText(R.string.save, TextView.BufferType.SPANNABLE)
        }

        // Fill inputs
        findViewById<EditText>(R.id.name_input).setText(name, TextView.BufferType.SPANNABLE)
        findViewById<EditText>(R.id.description_input).setText(description, TextView.BufferType.SPANNABLE)
        findViewById<EditText>(R.id.barcode_input).setText(barcode, TextView.BufferType.SPANNABLE)

        findViewById<Button>(R.id.save_button).setOnClickListener {
            val newName = findViewById<EditText>(R.id.name_input).text.toString()
            val newDescription = findViewById<EditText>(R.id.description_input).text.toString()

            @Serializable
            data class UpdateBaseItemRequest(val name: String, val description: String, val barcode: String?, val image_path: String?, val shelf_id: Int?)

            @Serializable
            data class CreateBaseItemResponse(val id: Int)

            // Send create or update request
            Api.makeRequest(this, {
                val request = UpdateBaseItemRequest(newName, newDescription, barcode, null, null)

                return@makeRequest when (id) {
                    0 -> api.requestJson<UpdateBaseItemRequest, CreateBaseItemResponse>("POST", "/base-items", request)
                    else -> api.requestJson<UpdateBaseItemRequest, Boolean>("PUT", "/base-items/${id}", request)
                }
            }) { response ->
                if (response is CreateBaseItemResponse) {
                    // View product immediately after creation
                    val intent = Intent(this, ViewProductActivity::class.java)
                    intent.putExtra("baseItemId", response.id)
                    startActivity(intent)
                } else {
                    // Return with new information after updating
                    val intent = Intent()
                    intent.putExtra("name", newName)
                    intent.putExtra("description", newDescription)
                    setResult(RESULT_OK, intent)
                }

                finish()
            }
        }

        findViewById<Button>(R.id.upload_image_button).setOnClickListener {
            // Prompt user to take picture or choose image
            val dialog = AlertDialog.Builder(this)
                .setTitle("Upload image")
                .setItems(arrayOf("Take a picture", "Select from album")) { dialog, which ->
                    val intent = when (which) {
                        // Take picture with camera
                        0 -> Intent(MediaStore.ACTION_IMAGE_CAPTURE)
                        // Select from album
                        1 -> {
                            val intent = Intent()
                            intent.type = "image/*"
                            intent.action = Intent.ACTION_GET_CONTENT
                            Intent.createChooser(intent, "Select image")
                        }
                        else -> return@setItems
                    }

                    val request = when (which) {
                        0 -> TAKE_PICTURE_REQUEST
                        1 -> PICK_IMAGE_REQUEST
                        else -> return@setItems
                    }

                    try {
                        startActivityForResult(intent, request)
                    } catch (e: ActivityNotFoundException) {
                        Toast.makeText(this, "No matching apps for this request", Toast.LENGTH_LONG).show()
                    }
                }
                .create()

            dialog.show()
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)

        if (resultCode != RESULT_OK || data == null) return

        // Retrieve selected image
        val bitmap = when (requestCode) {
            PICK_IMAGE_REQUEST -> {
                val inputStream = contentResolver.openInputStream(data.data!!)
                BitmapFactory.decodeStream(inputStream)
            }
            TAKE_PICTURE_REQUEST -> {
                data.extras!!.get("data") as Bitmap
            }
            else -> return
        }

        // Convert to PNG
        val outputStream = ByteArrayOutputStream()
        bitmap.compress(Bitmap.CompressFormat.PNG, 100, outputStream)
        outputStream.flush()
        outputStream.close()

        // Upload to backend
        Api.makeRequest(this, { api.request("POST", "/images", outputStream.toByteArray(), mapOf("Content-Type" to "image/png")) }) { http ->
            @Serializable
            data class UploadImageResponse(val path: String)

            val response = Json.decodeFromString<UploadImageResponse>(http.body)

            runOnUiThread {
                Toast.makeText(this, response.path, Toast.LENGTH_LONG).show()
            }
        }
    }
}
