package tech.mercantec.storagesystem.ui

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.*
import android.widget.Toast
import kotlinx.serialization.Serializable
import tech.mercantec.storagesystem.R
import tech.mercantec.storagesystem.services.Api

class BorrowedItemsFragment : Fragment() {
    lateinit var api: Api

    @Serializable
    data class SpecificItem(val id: Int, val description: String)

    @Serializable
    data class BaseItem(val id: Int, val name: String, val description: String, val image_url: String?)

    @Serializable
    data class Borrower(val id: Int, val name: String)

    @Serializable
    data class BorrowRequest(val id: Int, val loaned_to: Borrower, val base_item: BaseItem, val specific_item: SpecificItem, val accepted: Boolean)

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        api = Api(requireActivity())
    }

    override fun onResume() {
        super.onResume()

        Api.makeRequest(requireActivity(), { api.requestJson<Unit, ArrayList<BorrowRequest>>("GET", "/borrow-requests", null) }) {
            Toast.makeText(requireContext(), "Found ${it.size} borrow requests", Toast.LENGTH_SHORT).show()
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_borrowed_items, container, false)
    }
}
