namespace SwaggerRestApi.Models
{
    public class BorrowRequest
    {
        public int Id { get; set; }

        public int LoanTo { get; set; }

        public int SpecificItem { get; set; }

        public bool Accepted { get; set; }
    }
}
