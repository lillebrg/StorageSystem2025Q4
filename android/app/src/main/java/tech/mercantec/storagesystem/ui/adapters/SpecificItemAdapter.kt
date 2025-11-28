package tech.mercantec.storagesystem.ui.adapters

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.TextView
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.models.SpecificItem

class SpecificItemAdapter(val ctx: Context, val items: Array<SpecificItem>) : BaseAdapter() {
    val layoutInflater = ctx.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

    override fun getCount() = items.size

    override fun getItem(position: Int) = items[position]

    override fun getItemId(position: Int) = items[position].id.toLong()

    override fun getView(
        position: Int,
        convertView: View?,
        parent: ViewGroup?
    ): View? {
        val view = convertView ?: layoutInflater.inflate(R.layout.specific_items_list_item, parent, false)
        val item = getItem(position)

        view.findViewById<TextView>(R.id.barcode).setText(item.barcode, TextView.BufferType.SPANNABLE)
        view.findViewById<TextView>(R.id.description).setText(item.description, TextView.BufferType.SPANNABLE)

        if (item.loaned_to != null) {
            view.findViewById<TextView>(R.id.loaned_to).apply {
                visibility = View.VISIBLE
                setText(ctx.getString(R.string.loaned_to, item.loaned_to.name), TextView.BufferType.SPANNABLE)
            }
        }

        return view
    }
}
