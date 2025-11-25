using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO;
using SwaggerRestApi.Models.DTO.Rack;
using SwaggerRestApi.Models.DTO.Shelf;

namespace SwaggerRestApi.BusineesLogic
{
    public class RackLogic
    {
        private readonly RackDBAccess _rackdbaccess;
        private readonly StorageDBAccess _storagedbaccess;

        public RackLogic(RackDBAccess rackDBAcess, StorageDBAccess storageDBAccess)
        {
            _rackdbaccess = rackDBAcess;
            _storagedbaccess = storageDBAccess;
        }

        /// <summary>
        /// Gets a rack with all shelf on that rack
        /// </summary>
        /// <param name="id">The id of the rack to be returned</param>
        /// <returns>A rack with a list of shelfs</returns>
        public async Task<ActionResult<RackGet>> GetRack(int id)
        {
            var rack = await _rackdbaccess.GetRack(id);

            if (rack == null) { return new NotFoundObjectResult(new { message = "Could not find rack" }); }

            RackGet result = new RackGet
            {
                rack_no = rack.RackNo,
                shelves = new List<ShelfFromRack>()
            };

            foreach (var item in rack.Shelves)
            {
                ShelfFromRack shelf = new ShelfFromRack
                {
                    barcode = item.Barcode,
                    shelf_no = item.ShelfNo,
                    id = item.Id
                };
                result.shelves.Add(shelf);
            }

            return new OkObjectResult(result);
        }

        /// <summary>
        /// Updates a rack
        /// </summary>
        /// <param name="rackUpdate">Contains a rack number</param>
        /// <param name="id">The id of the object to be updated</param>
        /// <returns>True</returns>
        public async Task<ActionResult> UpdateRack(RackUpdate rackUpdate, int id)
        {
            var rack = await _rackdbaccess.GetRack(id);

            if (rack == null) { return new NotFoundObjectResult(new { message = "Could not find rack" }); }

            rack.RackNo = rackUpdate.rack_no;

            await _rackdbaccess.UpdateRack(rack);

            return new OkObjectResult(true);
        }

        /// <summary>
        /// Deletes a rack
        /// </summary>
        /// <param name="id">The id of the rack to be deleted</param>
        /// <returns>True</returns>
        public async Task<ActionResult> DeleteRack(int id)
        {
            var rack = await _rackdbaccess.GetRack(id);

            if (rack == null) { return new NotFoundObjectResult(new { message = "Could not find rack" }); }

            await _rackdbaccess.DeleteRack(rack);

            return new OkObjectResult(true);
        }

        /// <summary>
        /// Create a new rack
        /// </summary>
        /// <param name="rackCreate">Contains a rack numbur</param>
        /// <param name="storageId">The id of the storage the rack is in</param>
        /// <returns>An int that is the id of the rack that was created</returns>
        public async Task<ActionResult<CreateReturnInt>> CreateRack(RackCreate rackCreate, int storageId)
        {
            bool exist = await _rackdbaccess.CheckForExistingRackInStorage(rackCreate.rack_no, storageId);

            if (exist) { return new BadRequestObjectResult(new { message = "Rack already exists" }); }

            Rack rack = new Rack
            {
                RackNo = rackCreate.rack_no,
                Shelves = new List<Shelf>()
            };

            CreateReturnInt result = new CreateReturnInt();

            result.id = await _storagedbaccess.CreateRack(rack, storageId);

            if (result.id == -1) { return new NotFoundObjectResult(new { message = "Could not find storage" }); }

            return new OkObjectResult(result);
        }
    }
}
