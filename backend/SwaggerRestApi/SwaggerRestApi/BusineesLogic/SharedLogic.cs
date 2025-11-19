using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models.DTO.Barcode;
using SwaggerRestApi.Models.DTO.User;

namespace SwaggerRestApi.BusineesLogic
{
    public class SharedLogic
    {
        private readonly ItemDBAccess _itemdbaccess;
        private readonly UserDBAccess _userdbaccess;
        private readonly ShelfDBAccess _shelfdbaccess;
        private readonly IConfiguration _configuration;

        public SharedLogic(ItemDBAccess itemDBAccess, UserDBAccess userDBAccess, ShelfDBAccess shelfDBAccess, IConfiguration configuration)
        {
            _itemdbaccess = itemDBAccess;
            _userdbaccess = userDBAccess;
            _shelfdbaccess = shelfDBAccess;
            _configuration = configuration;
        }

        public async Task<ActionResult<ScannedBarcode>> GetScannedItem(string barcode)
        {
            var imageBaseURL = _configuration["ImageUrl"];

            var baseItem = await _itemdbaccess.GetBaseItemFromBarcode(barcode);

            if (baseItem.Id != 0)
            {
                ScannedBarcode scannedItem = new ScannedBarcode
                {
                    type = "base_item",
                    base_item = new BaseItemBarcode()
                };

                scannedItem.base_item.id = baseItem.Id;
                scannedItem.base_item.name = baseItem.Name;
                scannedItem.base_item.image_url = imageBaseURL + baseItem.Picture;
                scannedItem.base_item.description = baseItem.Description;
                scannedItem.base_item.specific_items = new List<SpecificItemFromBaseItemBarcode>();

                foreach (var item in baseItem.SpecificItems)
                {
                    SpecificItemFromBaseItemBarcode specificItemFromBaseItem = new SpecificItemFromBaseItemBarcode
                    {
                        id = item.Id,
                        description = item.Description
                    };
                    scannedItem.base_item.specific_items.Add(specificItemFromBaseItem);
                }

                return new OkObjectResult(scannedItem);
            }

            var specificItem = await _itemdbaccess.GetSpecificItemFromBarcode(barcode);

            if (specificItem.Id != 0)
            {
                ScannedBarcode scannedItem = new ScannedBarcode
                {
                    type = "specific_item",
                    specific_item = new SpecificItemBarcode()
                };

                scannedItem.specific_item.id = specificItem.Id;
                scannedItem.specific_item.description = specificItem.Description;
                scannedItem.specific_item.base_item = new BaseItemFromSpecificItemBarcode();
                scannedItem.specific_item.base_item.id = specificItem.BaseItemId;
                scannedItem.specific_item.base_item.name = specificItem.BaseItem.Name;
                scannedItem.specific_item.base_item.image_url = imageBaseURL + specificItem.BaseItem.Picture;
                scannedItem.specific_item.base_item.description = specificItem.BaseItem.Description;


                if (specificItem.BorrowedTo != null)
                {
                    var user = await _userdbaccess.GetUser((int)specificItem.BorrowedTo);

                    scannedItem.specific_item.loaned_to = new UserLoanedTo();
                    scannedItem.specific_item.loaned_to.id = user.Id;
                    scannedItem.specific_item.loaned_to.name = user.Name;
                }

                return new OkObjectResult(scannedItem);
            }

            var shelf = await _shelfdbaccess.GetShelfFromBarcode(barcode);

            if (shelf.Id != 0)
            {
                ScannedBarcode scannedItem = new ScannedBarcode
                {
                    type = "shelf",
                    shelf = new ShelfBarcode()
                };

                scannedItem.shelf.id = shelf.Id;
                scannedItem.shelf.shelf_no = shelf.ShelfNo;
                scannedItem.shelf.rack_id = shelf.RackId;
                scannedItem.shelf.BaseItems = new List<BaseItemFromShelfBarcode>();

                foreach (var item in shelf.BaseItems)
                {
                    BaseItemFromShelfBarcode baseItemFromShelf = new BaseItemFromShelfBarcode
                    {
                        id = item.Id,
                        name = item.Name,
                        description = item.Description,
                        image_url = imageBaseURL + item.Picture
                    };
                    scannedItem.shelf.BaseItems.Add(baseItemFromShelf);
                }

                return new OkObjectResult(scannedItem);
            }

            return new NotFoundObjectResult(new { message = "Could not find item" });
        }

        public async Task<string> CreateRandomBarcode()
        {
            bool flag = true;
            string barcode = "";
            Random random = new Random();

            while (flag)
            {
                bool itemFlag = false;
                bool shelfFlag = false;
                barcode = "150";
                int[] barcodeArray = new int[12];

                barcodeArray[0] = 1;
                barcodeArray[1] = 5;
                barcodeArray[2] = 0;


                for (int i = 3; i < 12; i++)
                {
                    barcodeArray[i] = random.Next(10);
                    barcode += barcodeArray[i];
                }


                int checkValue = 0;

                for (int i = 0; i < barcodeArray.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        checkValue += barcodeArray[i];
                    }
                    else
                    {
                        checkValue += barcodeArray[i] * 3;
                    }
                }

                checkValue = checkValue % 10;

                if (checkValue > 0)
                {
                    checkValue = 10 - checkValue;
                }

                barcode += checkValue;

                itemFlag = await _itemdbaccess.CheckForExistingBarcode(barcode);
                shelfFlag = await _shelfdbaccess.CheckForExistingBarcode(barcode);
                if (itemFlag && shelfFlag) { flag = false; }
            }

            return barcode;
        }
    }
}
