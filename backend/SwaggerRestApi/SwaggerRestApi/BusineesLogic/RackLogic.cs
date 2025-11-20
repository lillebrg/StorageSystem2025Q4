using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models.DTO.Rack;

namespace SwaggerRestApi.BusineesLogic
{
    public class RackLogic
    {
        private readonly RackDBAccess _rackdbaccess;

        public RackLogic(RackDBAccess rackDBAcess)
        {
            _rackdbaccess = rackDBAcess;
        }

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

        public async Task<ActionResult> UpdateRack(RackUpdate rackUpdate, int id)
        {
            var rack = await _rackdbaccess.GetRack(id);

            if (rack == null) { return new NotFoundObjectResult(new { message = "Could not find rack" }); }

            rack.RackNo = rackUpdate.rack_no;

            await _rackdbaccess.UpdateRack(rack);

            return new OkObjectResult(true);
        }

        public async Task<ActionResult> DeleteRack(int id)
        {
            var rack = await _rackdbaccess.GetRack(id);

            if (rack == null) { return new NotFoundObjectResult(new { message = "Could not find rack" }); }

            await _rackdbaccess.DeleteRack(rack);

            return new OkObjectResult(true);
        }
    }
}
