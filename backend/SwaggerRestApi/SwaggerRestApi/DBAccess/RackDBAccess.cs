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
            var rack = await _context.Racks.Include(r => r.Shelves).FirstOrDefaultAsync(r => r.Id == rackId);

            if (rack == null) { return -1; }

            rack.Shelves.Add(shelf);

            await _context.SaveChangesAsync();

            var savedShelf = await _context.Shelves.Where(s => s.RackId == rackId && s.ShelfNo == shelf.ShelfNo).FirstOrDefaultAsync();

            return savedShelf.Id;
        }

        public async Task<Rack> GetRack(int id)
        {
            var rack = await _context.Racks.Include(r => r.Shelves).FirstOrDefaultAsync(r => r.Id == id);

            return rack;
        }

        public async Task UpdateRack(Rack rack)
        {
            _context.Entry(rack).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRack(Rack rack)
        {
            _context.Racks.Remove(rack);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckForExistingRackInStorage(int rackNo, int storageId)
        {
            var shelf = await _context.Racks.Where(r => r.StorageId == storageId && r.RackNo == rackNo).FirstOrDefaultAsync();

            return shelf != null;
        }
    }
}
