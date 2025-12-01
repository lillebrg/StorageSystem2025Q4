namespace SwaggerRestApi.Models
{
    public class Rack
    {
        public int Id { get; set; }

        public int RackNo { get; set; }

        public List<Shelf> Shelves { get; set; }


        public int StorageId { get; set; }

        public Storage Storage { get; set; }
    }
}
