using Microsoft.EntityFrameworkCore;
using SwaggerRestApi.Models;

namespace SwaggerRestApi.DBAccess
{
    public class ItemDBAccess
    {
        private readonly DBContext _context;

        public ItemDBAccess(DBContext context)
        {
            _context = context;
        }

        public async Task<SpecificItem> GetSpecificItemAndBaseItem(int specificId)
        {
            var item = await _context.SpecificItems.Include(s => s.BaseItem).FirstOrDefaultAsync(s => s.Id == specificId);

            if (item == null) { return new SpecificItem(); }

            return item;
        }

        public async Task<List<BaseItem>> GetAllBaseItems(int limit, int offset, string search)
        {
            var items = await _context.BaseItems.Include(b => b.SpecificItems)
                .Where(b => b.Name.Contains(search))
                .OrderBy(b => b.Name).Skip(offset)
                .Take(limit).ToListAsync();

            return items;
        }

        public async Task<int> CreateBaseItem(BaseItem baseItem, int shelfId)
        {
            if (shelfId != 0)
            {
                var shelf = await _context.Shelves.Include(s => s.BaseItems).FirstOrDefaultAsync(s => s.Id == shelfId);

                if (shelf == null) { return -1; }

                shelf.BaseItems.Add(baseItem);
            }
            else
            {
                _context.BaseItems.Add(baseItem);
            }

            await _context.SaveChangesAsync();

            var savedBaseItem = await _context.BaseItems.Where(b => b.Name == baseItem.Name && b.ModelBarcode == baseItem.ModelBarcode).FirstOrDefaultAsync();

            return savedBaseItem.Id;
        }

        public async Task<BaseItem> GetBaseItem(int id)
        {
            var item = await _context.BaseItems.Include(b => b.SpecificItems).FirstOrDefaultAsync(b => b.Id == id);

            return item;
        }

        public async Task<BaseItem> GetBaseItemWithShelf(int id)
        {
            var item = await _context.BaseItems.Include(b => b.Shelf).FirstOrDefaultAsync(b => b.Id == id);

            return item;
        }

        public async Task UpdateBaseItem(BaseItem baseItem)
        {
            _context.Entry(baseItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBaseItem(BaseItem baseItem)
        {
            _context.BaseItems.Remove(baseItem);

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateSpecificItem(SpecificItem specificItem, int baseItemId)
        {
            var baseItem = await _context.BaseItems.Include(b => b.SpecificItems).FirstOrDefaultAsync(b => b.Id == baseItemId);

            if (baseItem == null) { return -1; }

            baseItem.SpecificItems.Add(specificItem);

            await _context.SaveChangesAsync();

            var savedSpecificItem = await _context.SpecificItems.Where(s => s.BaseItem.Id == baseItem.Id && s.Description == specificItem.Description).FirstOrDefaultAsync();

            return savedSpecificItem.Id;
        }
    }
}
