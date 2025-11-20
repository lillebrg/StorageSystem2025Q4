using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models.DTO;
using SwaggerRestApi.Models.DTO.Rack;
using SwaggerRestApi.Models.DTO.Storage;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    [Route("/storages")]
    public class StorageController : Controller
    {
        private readonly RackLogic _racklogic;
        private readonly StorageLogic _storagelogic;

        public StorageController(RackLogic rackLogic, StorageLogic storageLogic)
        {
            _racklogic = rackLogic;
            _storagelogic = storageLogic;
        }

        [HttpPost("{id}/racks")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult<CreateReturnInt>> CreateRack([FromBody] RackCreate rackCreate, int id)
        {
            return await _racklogic.CreateRack(rackCreate, id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult<List<StorageGet>>> GetAllStorages()
        {
            return await _storagelogic.GetAllStorages();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult<StorageGetWithRacks>> GetStorage(int id)
        {
            return await _storagelogic.GetStorage(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> UpdateStorage([FromBody] StorageUpdate storageUpdate, int id)
        {
            return await _storagelogic.UpdateStorage(storageUpdate, id);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult<CreateReturnInt>> CreateStorage([FromBody] StorageCreate storageCreate)
        {
            return await _storagelogic.CreateStorage(storageCreate);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> DeleteStorage(int id)
        {
            return await _storagelogic.DeleteRack(id);
        }
    }
}
