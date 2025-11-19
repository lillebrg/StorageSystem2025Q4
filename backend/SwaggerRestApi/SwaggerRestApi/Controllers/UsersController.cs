using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO.User;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : Controller
    {
        private readonly UserLogic _userlogic;

        public UsersController(UserLogic userLogic)
        {
            _userlogic = userLogic;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateUser([FromBody] UserCreate user)
        {
            return await _userlogic.RegisterUser(user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UsersDTO>>> GetAllUser()
        {
            return await _userlogic.GetAllUser();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserGet>> GetUser(int id)
        {
            return await _userlogic.GetUser(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUser([FromBody]UserUpdateAdmin user, int id)
        {
            return await _userlogic.EditUser(user, id);
        }

        [HttpPost("{id}/reset-password")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ResetPassword([FromBody]ResetPassword resetPassword, int id)
        {
            return await _userlogic.ResetPassword(resetPassword, id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            return await _userlogic.DeleteUser(id);
        }
    }
}
