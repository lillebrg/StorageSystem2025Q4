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
            var shelf = await _context.Shelves.Include(s => s.BaseItems).ThenInclude(b => b.SpecificItems).FirstOrDefaultAsync(s => s.Id == id);

            return shelf;
        }

        public async Task<bool> CheckForExistingShelfOnRack(int shelfNo, int rackId)
        {
            var shelf = await _context.Shelves.Where(s => s.RackId == rackId && s.ShelfNo == shelfNo).FirstOrDefaultAsync();

            return shelf != null;
        }

        public async Task<Shelf?> GetShelfFromBarcode(string barcode)
        {
            var shelf = await _context.Shelves.Include(s => s.BaseItems).FirstOrDefaultAsync(s => s.Barcode == barcode);

            return shelf;
        }

        public async Task UpdateShelf(Shelf shelf)
        {
            _context.Entry(shelf).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteShelf(Shelf shelf)
        {
            _context.Shelves.Remove(shelf);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckForExistingBarcode(string barcode)
        {
            var baseItems = await _context.Shelves.Where(b => b.Barcode.Contains(barcode)).FirstOrDefaultAsync();

            if (baseItems != null) { return false; }

            return true;
        }
    }
}
