namespace SwaggerRestApi.Models.DTO.Shelf
{
    public class ShelfGet
    {
        public int id { get; set; }

        public string barcode { get; set; }

        public int shelf_no { get; set; }

        public List<BaseItemFromShelf> base_items { get; set; }
    }
}
