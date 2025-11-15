namespace SwaggerRestApi.Models.DTO
{
    public class BaseItemDTO
    {
        public string name { get; set; }

        public string description { get; set; }

        public string? barcode { get; set; }

        public string? image_url { get; set; }

        public List<SpecificItemsDTO> specific_items { get; set; }
    }
}
