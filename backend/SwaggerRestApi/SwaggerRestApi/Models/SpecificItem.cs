namespace SwaggerRestApi.Models
{
    public class SpecificItem
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public string Barcode { get; set; }

        public int? BorrowedTo { get; set; }

        public int BaseItemId { get; set; }

        public BaseItem BaseItem { get; set; }
    }
}
