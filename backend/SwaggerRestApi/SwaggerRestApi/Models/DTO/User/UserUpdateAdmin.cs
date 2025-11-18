namespace SwaggerRestApi.Models.DTO.User
{
    public class UserUpdateAdmin
    {
        public string name { get; set; }

        public string email { get; set; }

        public string role { get; set; }

        public bool change_password_on_next_login { get; set; }
    }
}
