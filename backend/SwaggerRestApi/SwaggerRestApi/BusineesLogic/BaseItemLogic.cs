using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO;
using SwaggerRestApi.Models.DTO.BaseItems;
using SwaggerRestApi.Models.DTO.SpecificItems;
using SwaggerRestApi.Models.DTO.User;

namespace SwaggerRestApi.BusineesLogic
{
    public class BaseItemLogic
    {
        private readonly ItemDBAccess _itemdbaccess;
        private readonly ShelfDBAccess _shelfdbaccess;
        private readonly UserDBAccess _userdbaccess;

        public BaseItemLogic(ItemDBAccess itemDBAccess, ShelfDBAccess shelfDBAccess, UserDBAccess userDBAccess)
        {
            _itemdbaccess = itemDBAccess;
            _shelfdbaccess = shelfDBAccess;
            _userdbaccess = userDBAccess;
        }

        public async Task<ActionResult<List<BaseItemSearch>>> GetBaseItemBySearch(int limit, int offset, string? search)
        {
            if (search == null)
            {
                search = "";
            }

            var items = await _itemdbaccess.GetAllBaseItems(limit, offset, search);

            if (items.Count == 0) { return new OkObjectResult(new List<BaseItemSearch>()); }

            List<BaseItemSearch> result = new List<BaseItemSearch>();

            foreach (var item in items)
            {
                BaseItemSearch baseItem = new BaseItemSearch
                {
                    id = item.Id,
                    name = item.Name,
                    description = item.Description,
                    barcode = item.ModelBarcode,
                    image_url = item.Picture,
                    specific_items_count = item.SpecificItems.Count
                };

                result.Add(baseItem);
            }

            return new OkObjectResult(result);
        }

        public async Task<ActionResult<CreateReturnInt>> CreateBaseItem(BaseItemCreate newBaseItem)
        {
            BaseItem baseItem = new BaseItem();
            if (newBaseItem.shelf_id == null) { newBaseItem.shelf_id = 0; }


            baseItem.Name = newBaseItem.name;
            baseItem.Description = newBaseItem.description;
            baseItem.Picture = newBaseItem.image_path;
            baseItem.ModelBarcode = newBaseItem.barcode;

            baseItem.SpecificItems = new List<SpecificItem>();

            CreateReturnInt result = new CreateReturnInt();

            result.id = await _itemdbaccess.CreateBaseItem(baseItem, (int)newBaseItem.shelf_id);

            if (result.id == -1) { return new NotFoundObjectResult(new { message = "Could not find shelf" }); }

            return new OkObjectResult(result);
        }

        public async Task<ActionResult<BaseItemGet>> GetBaseItem(int id)
        {
            var baseItem = await _itemdbaccess.GetBaseItem(id);

            if (baseItem == null) { return new NotFoundObjectResult(new { message = "Could not fint base item" }); }

            BaseItemGet result = new BaseItemGet
            {
                id = baseItem.Id,
                name = baseItem.Name,
                description = baseItem.Description,
                barcode = baseItem.ModelBarcode,
                image_url = baseItem.Picture,
                specific_items = new List<SpecificItemsGet>()
            };

            foreach (var item in baseItem.SpecificItems)
            {
                SpecificItemsGet specificItem = new SpecificItemsGet();

                if (item.BorrowedTo != null)
                {
                    var user = await _userdbaccess.GetUser((int)item.BorrowedTo);
                    if (user != null && user.Id != 0)
                    {
                        specificItem.loaned_to = new UserLoanedTo();
                        specificItem.loaned_to.id = user.Id;
                        specificItem.loaned_to.name = user.Name;
                    }
                }

                specificItem.id = item.Id;
                specificItem.description = item.Description;

                result.specific_items.Add(specificItem);
            }

            return new OkObjectResult(result);
        }

        public async Task<ActionResult> UpdateBaseItem(BaseItemCreate baseItemCreate, int id)
        {
            var baseItem = await _itemdbaccess.GetBaseItem(id);

            if (baseItem == null) { return new NotFoundObjectResult(new { message = "Could not fint base item" }); }

            if (baseItemCreate.shelf_id != null && baseItemCreate.shelf_id != 0)
            {
                var shelf = await _shelfdbaccess.GetShelf((int)baseItemCreate.shelf_id);

                if (shelf == null || shelf.Id == 0) { return new NotFoundObjectResult(new { message = "Could not fint shelf" }); }

                baseItem.Shelf = shelf;
                baseItem.ShelfId = shelf.Id;
            }

            if (baseItemCreate.barcode != null && baseItemCreate.barcode != "") { baseItem.ModelBarcode = baseItemCreate.barcode; }
            if (baseItemCreate.image_path != null) { baseItem.Picture = baseItemCreate.image_path; }

            baseItem.Name = baseItemCreate.name;
            baseItem.Description = baseItemCreate.description;

            await _itemdbaccess.UpdateBaseItem(baseItem);

            return new OkObjectResult(true);
        }

        public async Task<ActionResult> DeleteBaseItem(int id)
        {
            var baseItem = await _itemdbaccess.GetBaseItem(id);

            if (baseItem == null) { return new NotFoundObjectResult(new { message = "Could not fint base item" }); }

            await _itemdbaccess.DeleteBaseItem(baseItem);

            return new OkObjectResult(true);
        }
    }
}
