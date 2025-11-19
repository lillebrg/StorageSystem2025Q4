using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO.BaseItems;
using SwaggerRestApi.Models.DTO.Shelf;

namespace SwaggerRestApi.BusineesLogic
{
    public class ShelfLogic
    {
        private readonly ShelfDBAccess _shelfdbaccess;
        private readonly IConfiguration _configuration;
        private readonly SharedLogic _sharedlogic;
        private readonly RackDBAccess _rackdbaccess;

        public ShelfLogic(ShelfDBAccess shelfDBAccess, IConfiguration configuration, SharedLogic sharedLogic, RackDBAccess rackDBAccess)
        {
            _shelfdbaccess = shelfDBAccess;
            _configuration = configuration;
            _sharedlogic = sharedLogic;
            _rackdbaccess = rackDBAccess;
        }

        public async Task<ActionResult<ShelfGet>> GetShelf(int id)
        {
            var imageBaseURL = _configuration["ImageUrl"];
            var shelf = await _shelfdbaccess.GetShelf(id);

            if (shelf == null) { return new NotFoundObjectResult(new { message = "Could not find shelf" }); }

            ShelfGet result = new ShelfGet
            {
                barcode = shelf.Barcode,
                id = shelf.Id,
                base_items = new List<BaseItemSearch>()
            };

            foreach (var item in shelf.BaseItems)
            {
                BaseItemSearch baseItem = new BaseItemSearch
                {
                    id = item.Id,
                    specific_items_count = item.SpecificItems.Count,
                    barcode = item.ModelBarcode,
                    description = item.Description,
                    image_url = imageBaseURL + item.Picture,
                    name = item.Name
                };

                result.base_items.Add(baseItem);
            }

            return new OkObjectResult(result);
        }

        public async Task<ActionResult> UpdateShelf(ShelfUpdate shelfUpdate, int id)
        {
            var shelf = await _shelfdbaccess.GetShelf(id);

            if (shelf == null) { return new NotFoundObjectResult(new { message = "Could not find shelf" }); }

            shelf.ShelfNo = shelfUpdate.shelf_no;

            await _shelfdbaccess.UpdateShelf(shelf);

            return new OkObjectResult(true);
        }

        public async Task<ActionResult> DeleteShelf(int id)
        {
            var shelf = await _shelfdbaccess.GetShelf(id);

            if (shelf == null) { return new NotFoundObjectResult(new { message = "Could not find shelf" }); }

            await _shelfdbaccess.DeleteShelf(shelf);

            return new OkObjectResult(true);
        }

        public async Task<ActionResult<ShelfCreateReturn>> CreateShelf(ShelfCreate shelfCreate, int rackId)
        {
            bool exist = await _shelfdbaccess.CheckForExistingShelfOnRack(shelfCreate.shelf_no, rackId);

            if (exist) { return new BadRequestObjectResult(new { message = "Shelf already exists" }); }

            Shelf shelf = new Shelf
            {
                ShelfNo = shelfCreate.shelf_no,
                Barcode = await _sharedlogic.CreateRandomBarcode(),
                BaseItems = new List<BaseItem>()
            };

            ShelfCreateReturn result = new ShelfCreateReturn();

            result.id = await _rackdbaccess.CreateShelf(shelf, rackId);
            result.barcode = shelf.Barcode;

            if (result.id == -1) { return new NotFoundObjectResult(new { message = "Could not find rack" }); }

            return new OkObjectResult(result);
        }
    }
}
