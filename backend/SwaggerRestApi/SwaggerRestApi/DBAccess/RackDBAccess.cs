using Microsoft.EntityFrameworkCore;
using SwaggerRestApi.Models;

namespace SwaggerRestApi.DBAccess
{
    public class RackDBAccess
    {
        private readonly DBContext _context;

        public RackDBAccess(DBContext dBContext)
        {
            _context = dBContext;
        }

        public async Task<int> CreateShelf(Shelf shelf, int rackId)
        {
            var rack = await _context.Racks.Include(b => b.Shelves).FirstOrDefaultAsync(b => b.Id == rackId);

            if (rack == null) { return -1; }

            rack.Shelves.Add(shelf);

            await _context.SaveChangesAsync();

            var savedShelf = await _context.Shelves.Where(s => s.RackId == rackId && s.ShelfNo == shelf.ShelfNo).FirstOrDefaultAsync();

            return savedShelf.Id;
        }
    }
}
