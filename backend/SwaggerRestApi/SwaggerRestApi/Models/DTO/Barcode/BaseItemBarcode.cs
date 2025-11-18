namespace SwaggerRestApi.Models.DTO.Barcode
{
    public class BaseItemBarcode
    {
        public int id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string? image_url { get; set; }

        public List<SpecificItemFromBaseItemBarcode> specific_items { get; set; }
    }
}
