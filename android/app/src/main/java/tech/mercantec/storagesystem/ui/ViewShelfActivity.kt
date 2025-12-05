package tech.mercantec.storagesystem.ui

import android.os.Bundle
import android.widget.*
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.ui.adapters.ShelfItemAdapter
import tech.mercantec.storagesystem.models.Shelf
import tech.mercantec.storagesystem.services.Api

class ViewShelfActivity : AppCompatActivity() {
    lateinit var api: Api

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_view_shelf)

        api = Api(this)

        val id = intent.extras!!.getInt("shelfId")


        Api.makeRequest(this, { api.requestJson<Unit, Shelf>("GET", "/shelves/$id", null) }) { shelf ->
            findViewById<TextView>(R.id.shelf_no).setText(getString(R.string.shelf_no, shelf.shelf_no), TextView.BufferType.SPANNABLE)

            findViewById<ListView>(R.id.shelf_items_list).adapter = ShelfItemAdapter(this, shelf.base_items)
        }
    }
}
