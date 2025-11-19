using Microsoft.EntityFrameworkCore;
using SwaggerRestApi.Models;

namespace SwaggerRestApi.DBAccess
{
    public class ShelfDBAccess
    {
        private readonly DBContext _context;

        public ShelfDBAccess(DBContext dBContext)
        {
            _context = dBContext;
        }

        public async Task<Shelf> GetShelf(int id)
        {
            var shelf = await _context.Shelves.FirstOrDefaultAsync(s => s.Id == id);

            return shelf;
        }

        public async Task<Shelf> GetShelfFromBarcode(string barcode)
        {
            var shelf = await _context.Shelves.Include(s => s.BaseItems).FirstOrDefaultAsync(s => s.Barcode == barcode);

            return shelf;
        }
    }
}
