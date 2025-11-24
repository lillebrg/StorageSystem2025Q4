package tech.mercantec.storagesystem.ui

import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import android.view.View
import android.widget.ImageView
import android.widget.TextView
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import kotlinx.serialization.Serializable
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.services.Api
import androidx.core.net.toUri

class ViewProductActivity : AppCompatActivity() {
    @Serializable
    data class Borrower(val id: Int, val name: String)

    @Serializable
    data class SpecificItem(val id: Int, val description: String, val loaned_to: Array<Borrower>)

    @Serializable
    data class BaseItem(val id: Int, val name: String, val description: String, val barcode: String?, val image_url: String?, val specific_items: Array<SpecificItem>)

    lateinit var api: Api
    lateinit var baseItem: BaseItem

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_view_product)

        api = Api(this)

        val id = intent.extras!!.getInt("baseItemId")

        Api.makeRequest(this, { api.requestJson<Unit, BaseItem>("GET", "/base-items/${id}", null) }, showLoading = false) {
            baseItem = it

            findViewById<TextView>(R.id.title).setText(baseItem.name, TextView.BufferType.SPANNABLE)
            findViewById<TextView>(R.id.description).setText(baseItem.description, TextView.BufferType.SPANNABLE)
            findViewById<TextView>(R.id.barcode).setText("Barcode: ${baseItem.barcode}", TextView.BufferType.SPANNABLE)
        }
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.view_product_actions, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem) = when (item.itemId) {
        R.id.edit -> {
            val intent = Intent(this, CreateProductActivity::class.java)
            startActivity(intent)

            true
        }
        else -> super.onOptionsItemSelected(item)
    }
}
