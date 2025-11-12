namespace SwaggerRestApi.Models.DTO
{
    public class UserDto
    {
        public string name { get; set; }

        public string email { get; set; }

        public string roles { get; set; }

        public List<int> borrowed_items { get; set; }

        public bool change_password_on_next_login { get; set; }
    }
}
