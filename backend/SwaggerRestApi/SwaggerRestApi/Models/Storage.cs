
namespace SwaggerRestApi.Models
{
    public class Storage
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public List<Rack> Racks { get; set; }
    }
}
