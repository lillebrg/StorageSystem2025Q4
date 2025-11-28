package tech.mercantec.storagesystem.models

import kotlinx.serialization.Serializable

@Serializable
data class Borrower(val id: Int, val name: String)

@Serializable
data class SpecificItem(val id: Int, val description: String, val barcode: String, val loaned_to: Borrower?)

@Serializable
data class BaseItem(val id: Int, var name: String, var description: String, var barcode: String?, var image_url: String?, var specific_items: ArrayList<SpecificItem>)
