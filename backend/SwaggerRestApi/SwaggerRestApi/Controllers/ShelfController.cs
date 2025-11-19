using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models.DTO.Shelf;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/shelves")]
    public class ShelfController : Controller
    {
        private readonly ShelfLogic _shelflogic;

        public ShelfController(ShelfLogic shelfLogic)
        {
            _shelflogic = shelfLogic;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult<ShelfGet>> GetShelf(int id)
        {
            return await _shelflogic.GetShelf(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> UpdateShelf([FromBody] ShelfUpdate shelfUpdate, int id)
        {
            return await _shelflogic.UpdateShelf(shelfUpdate, id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> DeleteShelf(int id)
        {
            return await _shelflogic.DeleteShelf(id);
        }
    }
}
