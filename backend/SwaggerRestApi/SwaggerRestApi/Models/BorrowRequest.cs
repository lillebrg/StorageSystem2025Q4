namespace SwaggerRestApi.Models
{
    public class BorrowRequest
    {
        public int Id { get; set; }

        public int LoanTo { get; set; }

        public int SpecificItemId { get; set; }

        public bool Accepted { get; set; }


        public SpecificItem SpecificItem { get; set; }

        public User User { get; set; }
    }
}
