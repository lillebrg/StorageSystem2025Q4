namespace SwaggerRestApi.Models.DTO.Barcode
{
    public class ScannedBarcode
    {
        public string type { get; set; }

        public BaseItemBarcode? base_item { get; set; }

        public SpecificItemBarcode? specific_item { get; set; }

        public ShelfBarcode? shelf { get; set; }


    }
}
