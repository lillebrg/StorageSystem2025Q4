using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO.Borrowed;
using SwaggerRestApi.Models.DTO.Rack;
using SwaggerRestApi.Models.DTO.User;

namespace SwaggerRestApi.BusineesLogic
{
    public class BorrowedRequestLogic
    {
        private readonly BorrowedRequestDBAccess _borrowedbaccess;
        private readonly ItemDBAccess _itemdbaccess;
        private readonly UserDBAccess _userdbaccess;
        private readonly IConfiguration _configuration;
        private readonly SharedLogic _sharedlogic;

        public BorrowedRequestLogic(
            BorrowedRequestDBAccess borrowedRequestDBAccess, 
            ItemDBAccess itemDBAccess, 
            UserDBAccess userDBAccess, 
            IConfiguration configuration, 
            SharedLogic sheredlogic)
        {
            _borrowedbaccess = borrowedRequestDBAccess;
            _itemdbaccess = itemDBAccess;
            _userdbaccess = userDBAccess;
            _configuration = configuration;
            _sharedlogic = sheredlogic;
        }

        public async Task<ActionResult<List<BorrowGet>>> GetAllBorrowRequest()
        {
            var imageBaseURL = _configuration["ImageUrl"];
            var borrowRequests = await _borrowedbaccess.GetAllBorrwRequests();

            List<BorrowGet> result = new List<BorrowGet>();

            foreach (var item in borrowRequests)
            {
                BorrowGet borrowGet = new BorrowGet
                {
                    accepted = item.Accepted,
                    id = item.Id,
                    base_item = new BaseItemFromBorrowed(),
                    loaned_to = new UserLoanedTo(),
                    specific_item = new SpecificItemFromBorrowed()
                };
                var specificItem = await _itemdbaccess.GetSpecificItem(item.SpecificItem);
                var baseItem = await _itemdbaccess.GetBaseItem(specificItem.BaseItemId);
                var user = await _userdbaccess.GetUser(item.LoanTo);

                borrowGet.base_item.id = baseItem.Id;
                borrowGet.base_item.name = baseItem.Name;
                borrowGet.base_item.description = baseItem.Description;
                borrowGet.base_item.image_url = imageBaseURL + baseItem.Picture;

                borrowGet.specific_item.id = specificItem.Id;
                borrowGet.specific_item.description = specificItem.Description;

                borrowGet.loaned_to.id = user.Id;
                borrowGet.loaned_to.name = user.Name;

                if (borrowGet.base_item.image_url == imageBaseURL) { borrowGet.base_item.image_url = null; }

                result.Add(borrowGet);
            }

            return new OkObjectResult(result);
        }

        public async Task<ActionResult> CreateBorrowRequest(BorrowRequestCreate borrowRequestCreate, int userId)
        {
            BorrowRequest borrowRequest = new BorrowRequest
            {
                Accepted = false,
                LoanTo = userId,
                SpecificItem = borrowRequestCreate.specific_item
            };


            bool exist = await _borrowedbaccess.CheckIfBorrowRequestExist(userId, borrowRequestCreate.specific_item);

            if (exist) { return new BadRequestObjectResult(new { message = "Borrow request already exist" }); }

            bool alreadyBorrowed = await _borrowedbaccess.CheckIfSpecificItemIsAlreadyBorrowed(borrowRequestCreate.specific_item);

            if (alreadyBorrowed) { return new BadRequestObjectResult(new { message = "Specific item is already borrowed by someone" }); }

            await _borrowedbaccess.CreateBorrowRequest(borrowRequest);

            var user = await _userdbaccess.GetUser(userId);
            var specificItem = await _itemdbaccess.GetSpecificItemAndBaseItem(borrowRequestCreate.specific_item);
            Notifications notification = new Notifications
            {
                SentBy = userId,
                Message = $"{user.Name} has created a borrow request for {specificItem.BaseItem.Name}"
            };

            await _sharedlogic.CreateNotification(notification);
            await _sharedlogic.SendAllNotifications();

            return new OkObjectResult(true);
        }

        public async Task<ActionResult> AcceptBorrowRequest(int id, int userId)
        {
            var borrowRequest = await _borrowedbaccess.GetBorrowRequest(id);

            if (borrowRequest == null) { return new NotFoundObjectResult(new { message = "Could not find borrow request" }); }

            if (borrowRequest.Accepted == true) { return new BadRequestObjectResult(new { message = "Borrow rquest is already accepted" }); }

            var user = await _userdbaccess.GetUser(borrowRequest.LoanTo);
            var specificItem = await _itemdbaccess.GetSpecificItem(borrowRequest.SpecificItem);

            user.BorrowedItems.Add(borrowRequest.SpecificItem);
            specificItem.BorrowedTo = borrowRequest.LoanTo;
            borrowRequest.Accepted = true;

            await _borrowedbaccess.UpdateBorrowRequest(borrowRequest);
            await _userdbaccess.UpdateUser(user);
            await _itemdbaccess.UpdateSpecificItem(specificItem);

            var borrowRequestsToBeDeleted = await _borrowedbaccess.GetAllBorrowRequestWithSpecificItemId(borrowRequest.SpecificItem);
            foreach (var item in borrowRequestsToBeDeleted)
            {
                _borrowedbaccess.DeleteBorrowRequest(item);
            }

            var acceptedBy = await _userdbaccess.GetUser(userId);
            var specificItemAndBaseItem = await _itemdbaccess.GetSpecificItemAndBaseItem(borrowRequest.SpecificItem);
            Notifications notification = new Notifications
            {
                SentBy = userId,
                Message = $"Your borrow request for {specificItemAndBaseItem.BaseItem.Name} was accepted by {acceptedBy.Name}",
                SentTo = user.Id
            };

            await _sharedlogic.CreateNotification(notification);
            await _sharedlogic.SendAllNotifications();

            return new OkObjectResult(true);
        }

        public async Task<ActionResult> RejectBorrowRequest(int id, int userId)
        {
            var borrowRequest = await _borrowedbaccess.GetBorrowRequest(id);

            if (borrowRequest == null || borrowRequest.Accepted == true) { return new NotFoundObjectResult(new { message = "Could not find borrow request" }); }

            await _borrowedbaccess.DeleteBorrowRequest(borrowRequest);

            var acceptedBy = await _userdbaccess.GetUser(userId);
            var specificItemAndBaseItem = await _itemdbaccess.GetSpecificItemAndBaseItem(borrowRequest.SpecificItem);
            Notifications notification = new Notifications
            {
                SentBy = userId,
                Message = $"Your borrow request for {specificItemAndBaseItem.BaseItem.Name} was rejected by {acceptedBy.Name}",
                SentTo = borrowRequest.LoanTo
            };

            await _sharedlogic.CreateNotification(notification);
            await _sharedlogic.SendAllNotifications();

            return new OkObjectResult(true);
        }

        public async Task<ActionResult> ReturnBorrowRequest(int id)
        {
            var borrowRequest = await _borrowedbaccess.GetBorrowRequest(id);

            if (borrowRequest == null) { return new NotFoundObjectResult(new { message = "Could not find borrow request" }); }

            var user = await _userdbaccess.GetUser(borrowRequest.LoanTo);
            var specificItem = await _itemdbaccess.GetSpecificItem(borrowRequest.SpecificItem);

            user.BorrowedItems.Remove(borrowRequest.SpecificItem);
            specificItem.BorrowedTo = null;

            await _userdbaccess.UpdateUser(user);
            await _itemdbaccess.UpdateSpecificItem(specificItem);

            await _borrowedbaccess.DeleteBorrowRequest(borrowRequest);

            return new OkObjectResult(true);
        }
    }
}
