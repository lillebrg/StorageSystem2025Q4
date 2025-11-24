package tech.mercantec.storagesystem.ui

import android.os.Bundle
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

        val barcode = intent.extras?.getString("barcode") ?: ""

        findViewById<EditText>(R.id.barcode_input).setText(barcode, TextView.BufferType.SPANNABLE)

        findViewById<Button>(R.id.create_button).setOnClickListener {
            val name = findViewById<EditText>(R.id.name_input).text.toString()
            val description = findViewById<EditText>(R.id.description_input).text.toString()

            @Serializable
            data class CreateBaseItemRequest(val name: String, val description: String, val barcode: String?, val image_path: String?, val shelf_id: Int?)

            @Serializable
            data class CreateBaseItemResponse(val id: Int)

            Api.makeRequest(this, {
                api.requestJson<CreateBaseItemRequest, CreateBaseItemResponse>(
                    "POST", "/base-items",
                    CreateBaseItemRequest(name, description, barcode, null, null)
                )
            }) {
                finish()
            }
        }
    }
}
