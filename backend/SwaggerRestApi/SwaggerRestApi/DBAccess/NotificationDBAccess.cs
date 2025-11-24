namespace SwaggerRestApi.DBAccess
{
    public class NotificationDBAccess
    {
        private readonly DBContext _context;

        public NotificationDBAccess(DBContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationSubscription()
    }
}
