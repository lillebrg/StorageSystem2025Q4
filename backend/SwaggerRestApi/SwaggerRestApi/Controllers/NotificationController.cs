using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models.DTO;
using System;
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
            ;

            

            return Ok();
        }
    }
}
