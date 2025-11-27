using SwaggerRestApi.Models.DTO.User;

namespace SwaggerRestApi.Models.DTO.Borrowed
{
    public class BorrowGet
    {
        public int id { get; set; }

        public UserLoanedTo loaned_to { get; set; }

        public BaseItemFromBorrowed base_item { get; set; }

        public SpecificItemFromBorrowed specific_item { get; set; }

        public bool accepted { get; set; }
    }
}
