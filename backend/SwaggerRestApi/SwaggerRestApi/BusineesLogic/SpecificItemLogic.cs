using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO;

namespace SwaggerRestApi.BusineesLogic
{
    public class SpecificItemLogic
    {
        private readonly ItemDBAccess _itemdbaccess;

        public SpecificItemLogic(ItemDBAccess itemmdbaccess)
        {
            _itemdbaccess = itemmdbaccess;
        }

        public async Task<ActionResult<CreateReturnInt>> CreateSpecificItem(SpecificItemsCreate specificItemsCreate, int baseItemId)
        {
            if (specificItemsCreate == null) { return new BadRequestObjectResult(new { message = "Specific Items cant be null" }); }

            SpecificItem specificItem = new SpecificItem();

            specificItem.Description = specificItemsCreate.description;
            specificItem.Barcode = await CreateRandomBarcode();

            CreateReturnInt result = new CreateReturnInt();

            result.id = await _itemdbaccess.CreateSpecificItem(specificItem, baseItemId);

            if (result.id == -1) { return new NotFoundObjectResult(new { message = "Could not find base item" }); }

            return new OkObjectResult(result);
        }

        private async Task<string> CreateRandomBarcode()
        {
            bool flag = true;
            string barcode = "";

            while (flag)
            {
                Random random = new Random();
                barcode = "";

                for (int i = 0; i < 13; i++)
                {
                    barcode += random.Next(10);
                }

                flag = await _itemdbaccess.CheckForExistingBarcode(barcode);
                flag = !flag;
            }

            return barcode;
        }
    }
}
