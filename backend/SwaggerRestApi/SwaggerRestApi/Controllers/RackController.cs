using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models.DTO.Rack;
using SwaggerRestApi.Models.DTO.Shelf;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/racks")]
    public class RackController : Controller
    {
        private readonly ShelfLogic _shelflogic;
        private readonly RackLogic _racklogic;

        public RackController(ShelfLogic shelfLogic, RackLogic rackLogic)
        {
            _shelflogic = shelfLogic;
            _racklogic = rackLogic;
        }

        [HttpPost("{id}/shelves")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult<ShelfCreateReturn>> CreateShelf([FromBody] ShelfCreate shelfCreate, int id)
        {
            return await _shelflogic.CreateShelf(shelfCreate, id);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult<RackGet>> GetRack(int id)
        {
            return await _racklogic.GetRack(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> UpdateRack([FromBody] RackUpdate rackUpdate, int id)
        {
            return await _racklogic.UpdateRack(rackUpdate, id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> DeleteRack(int id)
        {
            return await _racklogic.DeleteRack(id);
        }
    }
}
