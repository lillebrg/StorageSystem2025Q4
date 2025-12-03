using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models.DTO.Borrowed;
using System.Security.Claims;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/borrow-requests")]
    public class BorrowController : Controller
    {
        private readonly BorrowedRequestLogic _borrowedlogic;

        public BorrowController(BorrowedRequestLogic borrowedLogic)
        {
            _borrowedlogic = borrowedLogic;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult<List<BorrowGet>>> GetAllBorrowRequests()
        {
            return await _borrowedlogic.GetAllBorrowRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult> CreateBorrowRequest([FromBody] BorrowRequestCreate borrowRequest)
        {
            var claims = HttpContext.User.Claims;
            string userIdString = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            int userId = Convert.ToInt32(userIdString);
            return await _borrowedlogic.CreateBorrowRequest(borrowRequest, userId);
        }

        [HttpPost("{id}/accept")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> AcceptBorrowRequest(int id)
        {
            var claims = HttpContext.User.Claims;
            string userIdString = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            int userId = Convert.ToInt32(userIdString);
            return await _borrowedlogic.AcceptBorrowRequest(id, userId);
        }

        // send notification to user
        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult> RejectBorrowRequest(int id)
        {
            var claims = HttpContext.User.Claims;
            string userIdString = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            int userId = Convert.ToInt32(userIdString);
            return await _borrowedlogic.RejectBorrowRequest(id, userId);
        }

        [HttpPost("{id}/return")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> ReturnBorrowedItem(int id)
        {
            return await _borrowedlogic.ReturnBorrowRequest(id);
        }
    }
}
