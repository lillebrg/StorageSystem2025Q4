namespace SwaggerRestApi.Models.DTO.User
{
    public class UsersDTO
    {
        public int id { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public string role { get; set; }

        public int borrowed_items { get; set; }
    }
}
