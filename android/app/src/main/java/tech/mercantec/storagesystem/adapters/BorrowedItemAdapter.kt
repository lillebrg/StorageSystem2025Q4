package tech.mercantec.storagesystem.adapters

import android.content.Context
import android.content.Intent
import android.view.*
import android.widget.*
import tech.mercantec.storagesystem.models.BorrowRequest
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.ui.ViewProductActivity

class BorrowedItemAdapter(val ctx: Context, val items: ArrayList<BorrowRequest>) : BaseAdapter() {
    val layoutInflater = ctx.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

    override fun getCount() = items.size

    override fun getItem(position: Int) = items[position]

    override fun getItemId(position: Int) = items[position].id.toLong()

    override fun getView(
        position: Int,
        convertView: View?,
        parent: ViewGroup?
    ): View? {
        val view = convertView ?: layoutInflater.inflate(R.layout.borrowed_items_list_item, parent, false)
        val item = getItem(position)

        view.findViewById<TextView>(R.id.item_name).setText(item.base_item.name, TextView.BufferType.SPANNABLE)

        view.findViewById<TextView>(R.id.borrow_status).setText(if (item.loaned_to == null) "Waiting for approval" else "Approved", TextView.BufferType.SPANNABLE)

        if (item.loaned_to != null) {
            view.findViewById<Button>(R.id.return_button).apply {
                visibility = View.VISIBLE

                setOnClickListener {
                    Toast.makeText(ctx, "Return", Toast.LENGTH_SHORT).show()
                }
            }
        }

        view.findViewById<RelativeLayout>(R.id.container).setOnClickListener {
            val intent = Intent(ctx, ViewProductActivity::class.java)
            intent.putExtra("baseItemId", item.base_item.id)
            ctx.startActivity(intent)
        }

        return view
    }
}
