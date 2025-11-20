package tech.mercantec.storagesystem.ui

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.*
import tech.mercantec.storagesystem.R

class BorrowedItemsFragment : Fragment() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_borrowed_items, container, false)
    }
}
