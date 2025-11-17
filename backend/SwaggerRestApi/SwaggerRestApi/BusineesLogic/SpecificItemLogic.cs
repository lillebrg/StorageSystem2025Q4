using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO;

namespace SwaggerRestApi.BusineesLogic
{
    public class SpecificItemLogic
    {
        private readonly ItemDBAccess _itemdbaccess;
        private readonly SharedLogic _sharedlogic;

        public SpecificItemLogic(ItemDBAccess itemmdbaccess, SharedLogic sharedLogic)
        {
            _itemdbaccess = itemmdbaccess;
            _sharedlogic = sharedLogic;
        }

        public async Task<ActionResult<CreateReturnInt>> CreateSpecificItem(SpecificItemsCreate specificItemsCreate, int baseItemId)
        {
            if (specificItemsCreate == null) { return new BadRequestObjectResult(new { message = "Specific Items can not be null" }); }

            SpecificItem specificItem = new SpecificItem();

            specificItem.Description = specificItemsCreate.description;
            specificItem.Barcode = await _sharedlogic.CreateRandomBarcode();

            CreateReturnInt result = new CreateReturnInt();

            result.id = await _itemdbaccess.CreateSpecificItem(specificItem, baseItemId);

            if (result.id == -1) { return new NotFoundObjectResult(new { message = "Could not find base item" }); }

            return new OkObjectResult(result);
        }

        public async Task<ActionResult> UpdateSpecificItem(SpecificItemUpdate specificItemUpdate, int id)
        {
            if (specificItemUpdate == null) { return new BadRequestObjectResult(new { message = "Specific Item Desciption can not be null" }); }

            var specificItem = await _itemdbaccess.GetSpecificItem(id);

            if (specificItem == null) { return new NotFoundObjectResult(new { message = "Could not find specific item" }); }

            specificItem.Description = specificItemUpdate.description;

            await _itemdbaccess.UpdateSpecificItem(specificItem);

            return new OkObjectResult(true);
        }

        public async Task<ActionResult> DeleteSpecificItem(int id)
            {
            var specificItem = await _itemdbaccess.GetSpecificItem(id);

            if (specificItem == null) { return new NotFoundObjectResult(new { message = "Could not find specific item" }); }

            await _itemdbaccess.DeleteSpecificItem(specificItem);

            return new OkObjectResult(true);
        }
    }
}
