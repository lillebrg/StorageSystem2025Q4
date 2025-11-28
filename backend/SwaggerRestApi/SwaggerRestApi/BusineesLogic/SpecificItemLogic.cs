using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO;
using SwaggerRestApi.Models.DTO.SpecificItems;

namespace SwaggerRestApi.BusineesLogic
{
    public class SpecificItemLogic
    {
        private readonly ItemDBAccess _itemdbaccess;
        private readonly SharedLogic _sharedlogic;

        public SpecificItemLogic(ItemDBAccess itemDBAccess, SharedLogic sharedLogic)
        {
            _itemdbaccess = itemDBAccess;
            _sharedlogic = sharedLogic;
        }

        /// <summary>
        /// Create a new specific item
        /// </summary>
        /// <param name="specificItemsCreate">Contains a description and a barcode</param>
        /// <param name="baseItemId">The id of the base item that the specific item belongs to</param>
        /// <returns>An int that is the id of the rack that was created</returns>
        public async Task<ActionResult<SpecificItemsGet>> CreateSpecificItem(SpecificItemsCreate specificItemsCreate, int baseItemId)
        {
            if (specificItemsCreate == null) { return new BadRequestObjectResult(new { message = "Specific Items can not be null" }); }

            SpecificItem specificItem = new SpecificItem();

            specificItem.Description = specificItemsCreate.description;
            specificItem.Barcode = await _sharedlogic.CreateRandomBarcode();

            SpecificItemsGet result = new SpecificItemsGet();

            result.id = await _itemdbaccess.CreateSpecificItem(specificItem, baseItemId);
            result.description = specificItem.Description;
            result.barcode = specificItem.Barcode;
            result.loaned_to = null;

            if (result.id == -1) { return new NotFoundObjectResult(new { message = "Could not find base item" }); }

            return new OkObjectResult(result);
        }

        /// <summary>
        /// Updates a specific item
        /// </summary>
        /// <param name="specificItemUpdate">Contains a description and a barcode</param>
        /// <param name="id">The id of the specific item to be updated</param>
        /// <returns>True</returns>
        public async Task<ActionResult> UpdateSpecificItem(SpecificItemUpdate specificItemUpdate, int id)
        {
            if (specificItemUpdate == null) { return new BadRequestObjectResult(new { message = "Specific Item Desciption can not be null" }); }

            var specificItem = await _itemdbaccess.GetSpecificItem(id);

            if (specificItem == null) { return new NotFoundObjectResult(new { message = "Could not find specific item" }); }

            specificItem.Description = specificItemUpdate.description;

            await _itemdbaccess.UpdateSpecificItem(specificItem);

            return new OkObjectResult(true);
        }

        /// <summary>
        /// Deletes a specific item
        /// </summary>
        /// <param name="id">The id of the specific item to be deleted</param>
        /// <returns>True</returns>
        public async Task<ActionResult> DeleteSpecificItem(int id)
            {
            var specificItem = await _itemdbaccess.GetSpecificItem(id);

            if (specificItem == null) { return new NotFoundObjectResult(new { message = "Could not find specific item" }); }

            await _itemdbaccess.DeleteSpecificItem(specificItem);

            return new OkObjectResult(true);
        }
    }
}
