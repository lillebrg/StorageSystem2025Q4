package tech.mercantec.storagesystem.models

import kotlinx.serialization.Serializable

@Serializable
data class SpecificItem(val id: Int, val description: String)

@Serializable
data class BaseItem(val id: Int, val name: String, val description: String, val image_url: String?)

@Serializable
data class Borrower(val id: Int, val name: String)

@Serializable
data class BorrowRequest(val id: Int, val loaned_to: Borrower?, val base_item: BaseItem, val specific_item: SpecificItem, val accepted: Boolean)
