using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models.DTO;
using SwaggerRestApi.Models.DTO.BaseItems;
using SwaggerRestApi.Models.DTO.SpecificItems;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/base-items")]
    public class BaseItemController : Controller
    {
        private readonly BaseItemLogic _baseitemlogic;
        private readonly SpecificItemLogic _specificitemlogic;

        public BaseItemController(BaseItemLogic baseItemLogic, SpecificItemLogic specificItemLogic)
        {
            _baseitemlogic = baseItemLogic;
            _specificitemlogic = specificItemLogic;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult<List<BaseItemSearch>>> GetBaseItems(int limit, int offset, string? search)
        {
            return await _baseitemlogic.GetBaseItemBySearch(limit, offset, search);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult<CreateReturnInt>> CreateBaseItem([FromBody] BaseItemCreate baseItemCreate)
        {
            return await _baseitemlogic.CreateBaseItem(baseItemCreate);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult<BaseItemGet>> GetBaseItem(int id)
        {
            return await _baseitemlogic.GetBaseItem(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> UpdateBaseItem([FromBody]BaseItemCreate baseItemCreate, int id)
        {
            return await _baseitemlogic.UpdateBaseItem(baseItemCreate, id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> DeleteBaseItem(int id)
        {
            return await _baseitemlogic.DeleteBaseItem(id);
        }

        [HttpPost("{id}/specific-items")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult<CreateReturnInt>> CreateSpecificItems([FromBody]SpecificItemsCreate specificItems, int id)
        {
            return await _specificitemlogic.CreateSpecificItem(specificItems, id);
        }
    }
}
