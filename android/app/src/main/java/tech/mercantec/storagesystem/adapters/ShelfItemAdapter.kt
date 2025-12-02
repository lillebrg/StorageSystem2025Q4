package tech.mercantec.storagesystem.adapters

import android.content.Context
import android.content.Intent
import android.view.*
import android.widget.*
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.models.ShelfItem
import tech.mercantec.storagesystem.services.Api
import tech.mercantec.storagesystem.ui.ViewProductActivity

class ShelfItemAdapter(val ctx: Context, val items: ArrayList<ShelfItem>) : BaseAdapter() {
    val layoutInflater = ctx.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater
    val api = Api(ctx)

    override fun getCount() = items.size

    override fun getItem(position: Int) = items[position]

    override fun getItemId(position: Int) = items[position].id.toLong()

    override fun getView(
        position: Int,
        convertView: View?,
        parent: ViewGroup?
    ): View? {
        val view = convertView ?: layoutInflater.inflate(R.layout.shelf_items_list_item, parent, false)
        val item = getItem(position)

        view.findViewById<TextView>(R.id.name).setText(item.name, TextView.BufferType.SPANNABLE)

        if (item.description != null && item.description.isNotEmpty()) {
            view.findViewById<TextView>(R.id.description).apply {
                visibility = View.VISIBLE
                setText(item.description.split("\n")[0], TextView.BufferType.SPANNABLE)
            }
        }

        view.findViewById<LinearLayout>(R.id.container).setOnClickListener {
            val intent = Intent(ctx, ViewProductActivity::class.java)
            intent.putExtra("baseItemId", item.id)
            ctx.startActivity(intent)
        }

        return view
    }
}
