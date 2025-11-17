namespace SwaggerRestApi.Models.DTO
{
    public class SpecificItemsDTO
    {
        public int id { get; set; }

        public string? description { get; set; }

        public UserLoanedTo? loaned_to { get; set; }
    }
}
