using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO.User;
using System.Security.Claims;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/user")]
    public class CurrentUserController : Controller
    {

        private readonly UserLogic _userlogic;

        public CurrentUserController(UserLogic userLogic)
        {
            _userlogic = userLogic;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult<UserGet>> GetCurrentUser()
        {
            var claims = HttpContext.User.Claims;
            string userIdString = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            int userId = Convert.ToInt32(userIdString);
            return await _userlogic.GetUser(userId);
        }

        [HttpPost("change-password")]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePassword changePassword)
        {
            var claims = HttpContext.User.Claims;
            string userIdString = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            int userId = Convert.ToInt32(userIdString);
            return await _userlogic.ChangePassword(changePassword, userId);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] UserLogin login)
        {
            return await _userlogic.Login(login);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdate user)
        {
            var claims = HttpContext.User.Claims;
            string userIdString = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            int userId = Convert.ToInt32(userIdString);
            return await _userlogic.EditUser(user, userId);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> RefreshJWTToken([FromBody] RefreshTokenRequest tokenRequest)
        {
            return await _userlogic.RefreshJWTToken(tokenRequest);
        }

        [HttpPost("logout")]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult> DeleteRefreshToken([FromBody] RefreshTokenRequest tokenRequest)
        {
            return await _userlogic.DeleteRefreshToken(tokenRequest);
        }
    }
}
