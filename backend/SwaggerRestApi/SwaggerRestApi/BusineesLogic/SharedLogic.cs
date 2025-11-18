using SwaggerRestApi.DBAccess;

namespace SwaggerRestApi.BusineesLogic
{
    public class SharedLogic
    {
        private readonly ItemDBAccess _itemdbaccess;

        public SharedLogic(ItemDBAccess itemdbaccess)
        {
            _itemdbaccess = itemdbaccess;
        }

        public async Task<string> CreateRandomBarcode()
        {
            bool flag = true;
            string barcode = "";
            Random random = new Random();

            while (flag)
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

                flag = await _itemdbaccess.CheckForExistingBarcode(barcode);
                flag = !flag;
            }

            return barcode;
        }
    }
}
