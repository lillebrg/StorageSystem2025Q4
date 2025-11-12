namespace SwaggerRestApi.Models.DTO
{
    public class BorrowedItems
    {
        public int base_item_id { get; set; }

        public int specific_item_id { get; set; }

        public string base_item_name { get; set; }

        public string base_item_picture { get; set; }

        public string specific_item_description { get; set; }
    }
}
