using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO;
using System.Security.Claims;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/user")]
    public class CurrentUserController : Controller
    {

        private readonly UserLogic _logic;

        public CurrentUserController(UserLogic logic)
        {
            _logic = logic;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            var claims = HttpContext.User.Claims;
            string userIdString = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            int userId = Convert.ToInt32(userIdString);
            return await _logic.GetUser(userId);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePassword changePassword)
        {
            var claims = HttpContext.User.Claims;
            string userIdString = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            int userId = Convert.ToInt32(userIdString);
            return await _logic.ChangePassword(changePassword, userId);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLogin login)
        {
            return await _logic.Login(login);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdate user)
        {
            var claims = HttpContext.User.Claims;
            string userIdString = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            int userId = Convert.ToInt32(userIdString);
            return await _logic.EditUser(user, userId);
        }
    }
}
