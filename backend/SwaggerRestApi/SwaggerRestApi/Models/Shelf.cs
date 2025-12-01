namespace SwaggerRestApi.Models
{
    public class Shelf
    {
        public int Id { get; set; }

        public int ShelfNo { get; set; }

        public string Barcode { get; set; }

        public List<BaseItem> BaseItems { get; set; }


        public int RackId { get; set; }

        public Rack Rack { get; set; }
    }
}
