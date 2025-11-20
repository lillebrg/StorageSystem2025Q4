namespace SwaggerRestApi.Models.DTO.Barcode
{
    public class ShelfBarcode
    {
        public int id { get; set; }

        public int shelf_no { get; set; }

        public int rack_id { get; set; }

        public List<BaseItemFromShelfBarcode> base_items { get; set; }

    }
}
