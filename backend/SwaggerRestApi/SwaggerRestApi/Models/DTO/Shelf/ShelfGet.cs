using SwaggerRestApi.Models.DTO.BaseItems;

namespace SwaggerRestApi.Models.DTO.Shelf
{
    public class ShelfGet
    {
        public int id { get; set; }

        public string barcode { get; set; }

        public List<BaseItemSearch> base_items { get; set; }
    }
}
