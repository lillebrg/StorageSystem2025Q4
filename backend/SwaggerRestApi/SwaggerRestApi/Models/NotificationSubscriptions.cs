namespace SwaggerRestApi.Models
{
    public class NotificationSubscriptions
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Endpoint { get; set; }

        public string P256dh { get; set; }

        public string Auth { get; set; }
    }
}
