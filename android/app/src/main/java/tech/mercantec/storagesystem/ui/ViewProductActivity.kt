package tech.mercantec.storagesystem.ui

import android.app.ComponentCaller
import android.content.Intent
import android.graphics.drawable.Drawable
import android.os.Bundle
import android.view.*
import android.widget.*
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import kotlinx.serialization.Serializable
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.services.Api
import java.io.InputStream
import java.net.URL
import kotlin.concurrent.thread

class ViewProductActivity : AppCompatActivity() {
    @Serializable
    data class Borrower(val id: Int, val name: String)

    @Serializable
    data class SpecificItem(val id: Int, val description: String, val loaned_to: Array<Borrower>)

    @Serializable
    data class BaseItem(val id: Int, var name: String, var description: String, var barcode: String?, var image_url: String?, var specific_items: Array<SpecificItem>)

    lateinit var api: Api
    lateinit var baseItem: BaseItem

    val UPDATE_PRODUCT_REQUEST = 0

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_view_product)

        api = Api(this)

        val id = intent.extras!!.getInt("baseItemId")

        Api.makeRequest(this, { api.requestJson<Unit, BaseItem>("GET", "/base-items/${id}", null) }, showLoading = false) {
            baseItem = it

            showProductInfo()

            if (baseItem.image_url != null) {
                thread {
                    val stream = URL(baseItem.image_url!!).content as InputStream
                    val drawable = Drawable.createFromStream(stream, "src")

                    runOnUiThread {
                        findViewById<ImageView>(R.id.image).apply {
                            visibility = View.VISIBLE
                            setImageDrawable(drawable)
                        }
                    }
                }
            }
        }
    }

    private fun showProductInfo() {
        findViewById<TextView>(R.id.title).setText(baseItem.name, TextView.BufferType.SPANNABLE)
        findViewById<TextView>(R.id.description).setText(baseItem.description, TextView.BufferType.SPANNABLE)
        findViewById<TextView>(R.id.barcode).setText(getString(R.string.barcode_label, baseItem.barcode), TextView.BufferType.SPANNABLE)
    }

    override fun onActivityResult(
        requestCode: Int,
        resultCode: Int,
        data: Intent?,
        caller: ComponentCaller
    ) {
        super.onActivityResult(requestCode, resultCode, data, caller)

        // Update information after returning from editing the product
        if (requestCode == UPDATE_PRODUCT_REQUEST) {
            data?.run {
                baseItem.name = getStringExtra("name") ?: baseItem.name
                baseItem.description = getStringExtra("description") ?: baseItem.description
            }

            showProductInfo()
        }
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.view_product_actions, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem) = when (item.itemId) {
        R.id.edit -> {
            val intent = Intent(this, CreateProductActivity::class.java)
            intent.putExtra("id", baseItem.id)
            intent.putExtra("name", baseItem.name)
            intent.putExtra("description", baseItem.description)
            intent.putExtra("barcode", baseItem.barcode)
            intent.putExtra("imageUrl", baseItem.image_url)
            startActivityForResult(intent, UPDATE_PRODUCT_REQUEST)

            true
        }
        else -> super.onOptionsItemSelected(item)
    }
}
