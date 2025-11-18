using SwaggerRestApi.Models.DTO.User;

namespace SwaggerRestApi.Models.DTO.Barcode
{
    public class SpecificItemBarcode
    {
        public int id { get; set; }

        public string? description { get; set; }

        public UserLoanedTo? loaned_to { get; set; }
    }
}
