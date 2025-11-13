namespace SwaggerRestApi.Models.DTO
{
    public class BaseItemSearch
    {
        public int id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public Int64? barcode { get; set; }

        public string? image_url { get; set; }

        public int specific_items_count { get; set; }
    }
}
