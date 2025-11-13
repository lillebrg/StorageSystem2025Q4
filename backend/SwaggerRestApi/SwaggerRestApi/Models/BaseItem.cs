using System.Data.Common;

namespace SwaggerRestApi.Models
{
    public class BaseItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string? Picture { get; set; }

        public string? ModelBarcode { get; set; }

        public List<SpecificItem> SpecificItems { get; set; }

        public int? ShelfId { get; set; }

        public Shelf? Shelf { get; set; }
    }
}
