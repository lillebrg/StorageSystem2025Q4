namespace SwaggerRestApi.Models
{
    public class Notifications
    {
        public int Id { get; set; }

        public int SentBy { get; set; }

        public int? SentTo { get; set; }

        public string Message { get; set; }
    }
}
