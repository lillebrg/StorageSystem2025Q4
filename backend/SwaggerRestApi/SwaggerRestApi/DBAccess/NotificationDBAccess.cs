using Microsoft.EntityFrameworkCore;
using SwaggerRestApi.Models;

namespace SwaggerRestApi.DBAccess
{
    public class NotificationDBAccess
    {
        private readonly DBContext _context;

        public NotificationDBAccess(DBContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationSubscription(NotificationSubscriptions notificationSubscription)
        {
            _context.NotificationSubscriptions.Add(notificationSubscription);

            await _context.SaveChangesAsync();
        }

        public async Task CreateNotification(Notifications notification)
        {
            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Notifications>> GetNotifications()
        {
            var notifications = await _context.Notifications.ToListAsync();

            return notifications;
        }

        public async Task<List<NotificationSubscriptions>> GetAllNotificationSubscriptions()
        {
            var notificationsubs = await _context.NotificationSubscriptions.ToListAsync();

            return notificationsubs;
        }

        public async Task<NotificationSubscriptions> GetNotificationSubscription(int userId)
        {
            var notificationsubs = await _context.NotificationSubscriptions.FirstOrDefaultAsync(ns => ns.UserId == userId);

            return notificationsubs;
        }

        public async Task DeleteNotificationSubscription(NotificationSubscriptions notificationSubscription)
        {
            _context.NotificationSubscriptions.Remove(notificationSubscription);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotification(Notifications notification)
        {
            _context.Notifications.Remove(notification);

            await _context.SaveChangesAsync();
        }
    }
}
