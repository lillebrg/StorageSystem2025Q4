using SwaggerRestApi.Models.DTO.SpecificItems;

namespace SwaggerRestApi.Models.DTO.BaseItems
{
    public class BaseItemGet
    {
        public int id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string? barcode { get; set; }

        public string? image_url { get; set; }

        public List<SpecificItemsGet> specific_items { get; set; }
    }
}
