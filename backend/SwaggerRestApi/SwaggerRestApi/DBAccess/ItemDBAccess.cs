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
    }
}
