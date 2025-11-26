package tech.mercantec.storagesystem.ui

import android.os.Bundle
import android.content.Intent
import android.widget.*
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import kotlinx.serialization.Serializable
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.services.Api

class CreateProductActivity : AppCompatActivity() {
    val api = Api(this)

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
    }
}
