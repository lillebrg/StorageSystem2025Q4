using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models.DTO;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/specific-items")]
    public class SpecificItemController : Controller
    {
        private readonly SpecificItemLogic _specificitemlogic;

        public SpecificItemController(SpecificItemLogic specificItemLogic)
        {
            _specificitemlogic = specificItemLogic;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> UpdateSpecificItem([FromBody] SpecificItemUpdate specificItemUpdate, int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> DeleteSpecificItem(int id)
        {
            return Ok();
        }
    }
}
