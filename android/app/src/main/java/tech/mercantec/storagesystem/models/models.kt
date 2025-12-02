package tech.mercantec.storagesystem.models

import kotlinx.serialization.Serializable

@Serializable
data class BorrowedSpecificItem(val id: Int, val description: String)

@Serializable
data class BorrowedBaseItem(val id: Int, val name: String, val description: String, val image_url: String?)

@Serializable
data class Borrower(val id: Int, val name: String)

@Serializable
data class BorrowRequest(val id: Int, val loaned_to: Borrower?, val base_item: BorrowedBaseItem, val specific_item: BorrowedSpecificItem, val accepted: Boolean)

@Serializable
data class SpecificItem(val id: Int, var description: String, var barcode: String, val loaned_to: Borrower?)

@Serializable
data class BaseItem(val id: Int, var name: String, var description: String, var barcode: String?, var image_url: String?, var specific_items: ArrayList<SpecificItem>)
