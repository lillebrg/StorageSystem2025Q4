using SwaggerRestApi.Models.DTO.User;

namespace SwaggerRestApi.Models.DTO.SpecificItems
{
    public class SpecificItemsGet
    {
        public int id { get; set; }

        public string barcode { get; set; }

        public string? description { get; set; }

        public UserLoanedTo? loaned_to { get; set; }
    }
}
