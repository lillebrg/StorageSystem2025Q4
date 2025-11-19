namespace SwaggerRestApi.Models.DTO.User
{
    public class ChangePassword
    {
        public string current_password { get; set; }

        public string new_password { get; set; }
    }
}
