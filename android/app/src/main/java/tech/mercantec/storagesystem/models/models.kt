package tech.mercantec.storagesystem.models

import kotlinx.serialization.Serializable

@Serializable
data class ShelfItem(val id: Int, val name: String, val description: String, val barcode: String, val image_url: String?, val specific_items_count: Int, val specific_items_available_count: Int)

@Serializable
data class Shelf(val id: Int, val barcode: String, val shelf_no: Int, val base_items: ArrayList<ShelfItem>)
