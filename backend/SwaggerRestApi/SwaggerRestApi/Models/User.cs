namespace SwaggerRestApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<string> Roles { get; set; }

        public List<int> OnLoanItems { get; set; }

        public bool ChangePasswordOnNextLogin { get; set; }
    }
}
