namespace SwaggerRestApi.Models.DTO.Rack
{
    public class RackGet
    {
        public int rack_no { get; set; }

        public List<ShelfFromRack> shelves { get; set; }
    }
}
