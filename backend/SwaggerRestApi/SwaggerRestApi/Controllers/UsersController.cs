using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : Controller
    {
        private readonly UserLogic _logic;

        public UsersController(UserLogic logic)
        {
            _logic = logic;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateUser([FromBody] UserCreate user)
        {
            return await _logic.RegisterUser(user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UsersDTO>>> GetAllUser()
        {
            return await _logic.GetAllUser();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            return await _logic.GetUser(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUser([FromBody]UserUpdateAdmin user, int id)
        {
            return await _logic.EditUser(user, id);
        }

        [HttpPost("{id}/reset-password")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ResetPassword([FromBody]ResetPassword resetPassword, int id)
        {
            return await _logic.ResetPassword(resetPassword, id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            return await _logic.DeleteUser(id);
        }
    }
}
