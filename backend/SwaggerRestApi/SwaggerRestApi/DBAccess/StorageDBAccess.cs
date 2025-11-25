using Microsoft.EntityFrameworkCore;
using SwaggerRestApi.Models;

namespace SwaggerRestApi.DBAccess
{
    public class StorageDBAccess
    {
        private readonly DBContext _context;

        public StorageDBAccess(DBContext context)
        {
            _context = context;
        }

        public async Task<int> CreateRack(Rack rack, int storageId)
        {
            var storage = await _context.Storages.Include(s => s.Racks).FirstOrDefaultAsync(s => s.Id == storageId);

            if (storage == null) { return -1; }

            storage.Racks.Add(rack);

            await _context.SaveChangesAsync();

            var savedRack = await _context.Racks.Where(r => r.StorageId == storageId && r.RackNo == rack.RackNo).FirstOrDefaultAsync();

            return savedRack.Id;
        }

        public async Task<Storage> GetStorage(int id)
        {
            var storage = await _context.Storages.Include(s => s.Racks).FirstOrDefaultAsync(s => s.Id == id);

            return storage;
        }

        public async Task<List<Storage>> GetAllStorages()
        {
            var storages = await _context.Storages.ToListAsync();

            return storages;
        }

        public async Task UpdateStorage(Storage storage)
        {
            _context.Entry(storage).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteStorage(Storage storage)
        {
            _context.Storages.Remove(storage);

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateStorage(Storage storage)
        {
            _context.Storages.Add(storage);

            await _context.SaveChangesAsync();

            var savedStorage = await _context.Storages.Where(s => s.Name == storage.Name).FirstOrDefaultAsync();

            return savedStorage.Id;
        }

        public async Task<bool> CheckForExistingStorage(string name)
        {
            var storage = await _context.Storages.Where(s => s.Name == name).FirstOrDefaultAsync();

            return storage != null;
        }
    }
}
