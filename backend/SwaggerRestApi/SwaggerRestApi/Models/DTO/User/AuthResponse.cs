namespace SwaggerRestApi.Models.DTO.User
{
    public class AuthResponse
    {
        public string name { get; set; }

        public string email { get; set; }

        public string role { get; set; }

        public string access_token { get; set; }
    }
}
