using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO;
using SwaggerRestApi.Models.DTO.Barcode;
using SwaggerRestApi.Models.DTO.Borrowed;
using SwaggerRestApi.Models.DTO.User;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using WebPush;

namespace SwaggerRestApi.BusineesLogic
{
    public class SharedLogic
    {
        private readonly ItemDBAccess _itemdbaccess;
        private readonly UserDBAccess _userdbaccess;
        private readonly ShelfDBAccess _shelfdbaccess;
        private readonly IConfiguration _configuration;
        private readonly NotificationDBAccess _notificationdbaccess;

        public SharedLogic(ItemDBAccess itemDBAccess, UserDBAccess userDBAccess, ShelfDBAccess shelfDBAccess, IConfiguration configuration, NotificationDBAccess notificationDBAccess)
        {
            _itemdbaccess = itemDBAccess;
            _userdbaccess = userDBAccess;
            _shelfdbaccess = shelfDBAccess;
            _configuration = configuration;
            _notificationdbaccess = notificationDBAccess;
        }

        /// <summary>
        /// Finds out what kind of barcode you have scanned and returns an object with the data on what you have scanned
        /// </summary>
        /// <param name="barcode">The barcode that is to be found</param>
        /// <returns>A type with what kind of object it is and the data in that object while the other 2 is null</returns>
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

                if (scannedItem.base_item.image_url == imageBaseURL) { scannedItem.base_item.image_url = null; }

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

                if (scannedItem.specific_item.base_item.image_url == imageBaseURL) { scannedItem.specific_item.base_item.image_url = null; }

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

            if (shelf != null)
            {
                ScannedBarcode scannedItem = new ScannedBarcode
                {
                    type = "shelf",
                    shelf = new ShelfBarcode()
                };

                scannedItem.shelf.id = shelf.Id;
                scannedItem.shelf.shelf_no = shelf.ShelfNo;
                scannedItem.shelf.rack_id = shelf.RackId;
                scannedItem.shelf.base_items = new List<BaseItemFromShelfBarcode>();

                foreach (var item in shelf.BaseItems)
                {
                    BaseItemFromShelfBarcode baseItemFromShelf = new BaseItemFromShelfBarcode
                    {
                        id = item.Id,
                        name = item.Name,
                        description = item.Description,
                        image_url = imageBaseURL + item.Picture
                    };

                    if (baseItemFromShelf.image_url == imageBaseURL) { baseItemFromShelf.image_url = null; }

                    scannedItem.shelf.base_items.Add(baseItemFromShelf);
                }

                return new OkObjectResult(scannedItem);
            }

            return new NotFoundObjectResult(new { message = "Could not find item" });
        }

        /// <summary>
        /// Creates a random barcode using the EAN-13 standard and have set the country code to an unused one 
        /// so it should not be possible to find any of the generated ones elsewhere
        /// </summary>
        /// <returns>A string with 13 numbers</returns>
        public async Task<string> CreateRandomBarcode()
        {
            string barcode = "";
            Random random = new Random();
            bool itemFlag = false;
            bool shelfFlag = false;

            while (!itemFlag && !shelfFlag)
            {
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
            }

            return barcode;
        }

        /// <summary>
        /// Saves an users subscription info in the database so it can be used to sent a notifications later
        /// </summary>
        /// <param name="subscribe">The info to sent a notification to a user</param>
        /// <param name="userId">The user that that sends that sends the subscribe info</param>
        /// <returns>True</returns>
        public async Task<ActionResult> CreateNotificationSubscription(NotificationSub subscribe, int userId)
        {
            NotificationSubscriptions notificationSubscription = new NotificationSubscriptions
            {
                Auth = subscribe.auth,
                Endpoint = subscribe.endpoint,
                P256dh = subscribe.p256dh,
                UserId = userId
            };

            await _notificationdbaccess.CreateNotificationSubscription(notificationSubscription);

            SendAllNotifications();

            return new OkObjectResult(true);
        }

        /// <summary>
        /// Creates a notification that is then saved in the database
        /// </summary>
        /// <param name="notification">Contains who sent it what the message is and if it should be sent to someone specific it is also there</param>
        /// <returns>True</returns>
        public async Task<ActionResult> CreateNotification(Notifications notification)
        {
            await _notificationdbaccess.CreateNotification(notification);

            return new OkObjectResult(true);
        }

        /// <summary>
        /// Sends all notifications that are saved in our database if it is to someone specific and cant find them it will just keep them saved
        /// if it could be sent it is deleted from the database
        /// </summary>
        public async Task SendAllNotifications()
        {
            var notifications = await _notificationdbaccess.GetNotifications();

            foreach (var item in notifications)
            {
                if (item.SentTo != null)
                {
                    var subscription = await _notificationdbaccess.GetNotificationSubscription((int)item.SentTo);

                    if (subscription != null)
                    {
                        await SendNotification(subscription, item.Message);

                        _notificationdbaccess.DeleteNotification(item);
                    }
                }
                else
                {
                    var subsciptions = await _notificationdbaccess.GetAllNotificationSubscriptions();

                    foreach (var subscription in subsciptions)
                    {
                        var adminOrOperator = await _userdbaccess.CheckIfAdminOrOperator(subscription.UserId);

                        if (adminOrOperator)
                        {
                            await SendNotification(subscription, item.Message);
                        }
                    }

                    _notificationdbaccess.DeleteNotification(item);
                }
            }
        }

        /// <summary>
        /// Sends a notification to the user that have subscribed if it cant and gets back status code 410 
        /// the subscription gets deleted from the database
        /// </summary>
        /// <param name="subscription">What subscription to send the notifications to</param>
        /// <param name="message">What is the message that is in the notification</param>
        private async Task SendNotification(NotificationSubscriptions subscription, string message)
        {
            PushSubscription sub = new PushSubscription();
            sub.Auth = subscription.Auth;
            sub.P256DH = subscription.P256dh;
            sub.Endpoint = subscription.Endpoint;

            var subject = _configuration["Notification:Subject"];
            var publicKey = _configuration["Notification:PublicKey"];
            var privateKey = _configuration["Notification:PrivateKey"];
            var vapidDetails = new VapidDetails(subject, publicKey, privateKey);

            var webPushClient = new WebPushClient();
            try
            {
                await webPushClient.SendNotificationAsync(sub, message, vapidDetails);
            }
            catch (WebPushException exception)
            {
                if (exception.StatusCode == (HttpStatusCode)410)
                {
                    _notificationdbaccess.DeleteNotificationSubscription(subscription);
                    return;
                }

                throw;
            }
        }
    }
}
