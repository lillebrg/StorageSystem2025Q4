using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models.DTO.Shelf;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/racks")]
    public class RackController : Controller
    {
        private readonly ShelfLogic _shelflogic;

        public RackController(ShelfLogic shelfLogic)
        {
            _shelflogic = shelfLogic;
        }

        [HttpPost("{id}/shelves")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult<ShelfCreateReturn>> CreateShelf([FromBody] ShelfCreate shelfCreate, int id)
        {
            return await _shelflogic.CreateShelf(shelfCreate, id);
        }
    }
}
