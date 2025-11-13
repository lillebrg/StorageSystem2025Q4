namespace SwaggerRestApi.Models.DTO
{
    public class BaseItemCreate
    {
        public string name { get; set; }

        public string description { get; set; }

        public int? barcode { get; set; }

        public string? image_path { get; set; }

        public int? shelf_id { get; set; }
    }
}
