namespace SwaggerRestApi.Models.DTO.Storage
{
    public class StorageGetWithRacks
    {
        public int id { get; set; }

        public string name { get; set; }

        public List<RackFromStorage> racks { get; set; }
    }
}
