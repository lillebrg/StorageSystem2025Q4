package tech.mercantec.storagesystem.ui

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.*
import android.widget.ListView
import android.widget.Toast
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.adapters.BorrowedItemAdapter
import tech.mercantec.storagesystem.models.BorrowRequest
import tech.mercantec.storagesystem.services.Api

class BorrowedItemsFragment : Fragment() {
    lateinit var api: Api

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        api = Api(requireActivity())
    }

    override fun onResume() {
        super.onResume()

        loadItems()
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_borrowed_items, container, false)
    }

    private fun loadItems() {
        Api.makeRequest(requireActivity(), { api.requestJson<Unit, ArrayList<BorrowRequest>>("GET", "/borrow-requests", null) }) { items ->
            requireActivity().findViewById<ListView>(R.id.borrowed_items_list).adapter = BorrowedItemAdapter(requireActivity(), items) { id -> onReturnItem(id) }
        }
    }

    private fun onReturnItem(id: Int) {
        Api.makeRequest(requireActivity(), { api.requestJson<Unit, Boolean>("POST", "/borrow-requests/${id}/return", null) }) {
            loadItems()

            Toast.makeText(requireContext(), "Item has been returned", Toast.LENGTH_LONG).show()
        }
    }
}
