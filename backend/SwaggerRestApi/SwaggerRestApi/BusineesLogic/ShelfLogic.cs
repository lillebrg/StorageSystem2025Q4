using SwaggerRestApi.DBAccess;

namespace SwaggerRestApi.BusineesLogic
{
    public class ShelfLogic
    {
        private readonly ShelfDBAccess _shelfdbaccess;

        public ShelfLogic(ShelfDBAccess shelfDBAccess)
        {
            _shelfdbaccess = shelfDBAccess;
        }
    }
}
