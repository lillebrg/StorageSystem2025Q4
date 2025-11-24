package tech.mercantec.storagesystem.ui

import android.os.Bundle
import android.widget.*
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import tech.mercantec.storagesystem.R

class CreateProductActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_create_product)

        val barcode = intent.extras?.getString("barcode") ?: ""

        findViewById<EditText>(R.id.barcode_input).setText(barcode, TextView.BufferType.SPANNABLE)

        findViewById<Button>(R.id.create_button).setOnClickListener {

        }
    }
}
