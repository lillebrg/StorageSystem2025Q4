using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models.DTO;
using System;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using WebPush;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/notifications/subscribe")]
    public class NotificationController : Controller
    {
        private readonly SharedLogic _sharedlogic;

        public NotificationController(SharedLogic sharedLogic)
        {
            _sharedlogic = sharedLogic;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult> SubscribeToNotifications([FromBody] NotificationSub subscribe)
        {
            var claims = HttpContext.User.Claims;
            string userIdString = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            int userId = Convert.ToInt32(userIdString);
            return await _sharedlogic.CreateNotificationSubscription(subscribe, userId);
        }
    }
}
