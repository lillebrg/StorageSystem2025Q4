using Microsoft.EntityFrameworkCore;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using System.Threading.Tasks;

namespace SwaggerRestApi
{
    public class SeedData
    {
        private readonly DBContext _context;

        public SeedData(DBContext context)
        {
            _context = context;
        }

        public async Task StartUserData()
        {
            User newUser = new User
            {
                ChangePasswordOnNextLogin = false,
                Email = "admin",
                Name = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                Role = "Admin",
                BorrowedItems = new List<int>()
            };

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Role == "Admin");

            if (user == null || user.Id == 0) { _context.Users.Add(newUser); }

            await _context.SaveChangesAsync();
        }

        public async Task StartStorageData()
        {
            Storage newStorage = new Storage
            {
                Name = "Test",
                Racks = new List<Rack>()
            };
            Rack newRack = new Rack
            {
                RackNo = 1,
                Shelves = new List<Shelf>()
            };
            Shelf newShelf = new Shelf
            {
                ShelfNo = 1,
                Barcode = 4304493269774,
                BaseItems = new List<BaseItem>()
            };

            var storage = await _context.Storages.FirstOrDefaultAsync();

            if (storage == null || storage.Id == 0)
            {
                _context.Storages.Add(newStorage);
                await _context.SaveChangesAsync();
            }

            var rack = await _context.Racks.FirstOrDefaultAsync();

            if (rack == null || rack.Id == 0)
            {
                storage = await _context.Storages.FirstOrDefaultAsync();
                storage.Racks.Add(newRack);
                await _context.SaveChangesAsync();
            }

            var shelf = await _context.Shelves.FirstOrDefaultAsync();

            if (shelf == null || shelf.Id == 0)
            {
                rack = await _context.Racks.FirstOrDefaultAsync();
                rack.Shelves.Add(newShelf);
                await _context.SaveChangesAsync();
            }
        }
    }
}
