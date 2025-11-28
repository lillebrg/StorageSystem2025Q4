package tech.mercantec.storagesystem.ui

import android.app.AlertDialog
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
import tech.mercantec.storagesystem.models.*
import tech.mercantec.storagesystem.services.Api
import tech.mercantec.storagesystem.ui.adapters.SpecificItemAdapter
import java.io.InputStream
import java.net.URL
import kotlin.concurrent.thread

class ViewProductActivity : AppCompatActivity() {
    lateinit var api: Api
    lateinit var baseItem: BaseItem

    val UPDATE_PRODUCT_REQUEST = 0

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_view_product)

        api = Api(this)

        findViewById<Button>(R.id.add_specific_item_button).setOnClickListener {
            // Show dialog to add a specific item
            val layout = LinearLayout(this)
            layout.orientation = LinearLayout.VERTICAL
            layout.setPadding(60, 40, 60, 0)

            val editText = EditText(this)
            editText.hint = "Description (optional)"
            layout.addView(editText)

            val dialog = AlertDialog.Builder(this)
                .setTitle("Add item")
                .setView(layout)
                .setPositiveButton("Add") { dialog, which ->
                    createSpecificItem(editText.text.toString())
                }
                .setNegativeButton("Cancel") { dialog, which -> }
                .create()

            dialog.show()
        }

        findViewById<ListView>(R.id.specific_items_list).setOnItemLongClickListener { parent, view, position, id ->
            val item = parent.getItemAtPosition(position) as SpecificItem

            val activity = this
            val popup = PopupMenu(this, view)
            popup.apply {
                menuInflater.inflate(R.menu.specific_item_actions, menu)

                setOnMenuItemClickListener {
                    when (it.itemId) {
                        R.id.edit -> {
                            true
                        }
                        R.id.delete -> {
                            val dialog = AlertDialog.Builder(activity)
                                .setTitle("Delete item")
                                .setMessage("Delete this item? This action cannot be undone")
                                .setPositiveButton("Delete") { dialog, which ->
                                    Api.makeRequest(activity, { api.requestJson<Unit, Boolean>("DELETE", "/specific-items/${item.id}", null) }) {
                                        baseItem.specific_items.removeIf { it.id == item.id }

                                        showProductInfo()

                                        Toast.makeText(applicationContext, "Item deleted", Toast.LENGTH_SHORT).show()
                                    }
                                }
                                .setNegativeButton("Cancel") { dialog, which -> }
                                .create()

                            dialog.show()

                            true
                        }
                        else -> false
                    }
                }

                show()
            }

            true
        }
    }

    override fun onResume() {
        super.onResume()

        fetchProduct()
    }

    private fun fetchProduct() {
        val id = intent.extras!!.getInt("baseItemId")

        Api.makeRequest(this, { api.requestJson<Unit, BaseItem>("GET", "/base-items/${id}", null) }, showLoading = false) {
            baseItem = it

            showProductInfo()
        }
    }

    private fun createSpecificItem(description: String) {
        @Serializable
        data class CreateSpecificItemRequest(val description: String)

        val req = CreateSpecificItemRequest(description)
        Api.makeRequest(this, {
            api.requestJson<CreateSpecificItemRequest, SpecificItem>("POST", "/base-items/${baseItem.id}/specific-items", req)
        }) { response ->
            baseItem.specific_items.add(response)

            showProductInfo()
        }
    }

    private fun showProductInfo() {
        findViewById<TextView>(R.id.title).setText(baseItem.name, TextView.BufferType.SPANNABLE)
        findViewById<TextView>(R.id.description).setText(baseItem.description, TextView.BufferType.SPANNABLE)
        findViewById<TextView>(R.id.barcode).setText(getString(R.string.barcode_label, baseItem.barcode), TextView.BufferType.SPANNABLE)

        findViewById<ListView>(R.id.specific_items_list).apply {
            adapter = SpecificItemAdapter(applicationContext, baseItem.specific_items)

            val height = dpToPx(70) * adapter.count + dividerHeight * (adapter.count - 1)
            val params = layoutParams
            params.height = height
            layoutParams = params
            requestLayout()
        }

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

    private fun dpToPx(dp: Int): Int {
        return (dp * resources.displayMetrics.density).toInt()
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
            fetchProduct()
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
