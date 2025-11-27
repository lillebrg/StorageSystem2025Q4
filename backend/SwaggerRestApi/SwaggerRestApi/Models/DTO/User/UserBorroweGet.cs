using SwaggerRestApi.Models.DTO.Borrowed;

namespace SwaggerRestApi.Models.DTO.User
{
    public class UserBorroweGet
    {
        public int id { get; set; }

        public BaseItemFromBorrowed base_item { get; set; }

        public SpecificItemFromBorrowed specific_item { get; set; }

        public bool accepted { get; set; }
    }
}
