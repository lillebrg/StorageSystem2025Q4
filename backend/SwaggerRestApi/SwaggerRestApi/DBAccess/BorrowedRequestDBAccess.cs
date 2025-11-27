using Microsoft.EntityFrameworkCore;
using SwaggerRestApi.Models;

namespace SwaggerRestApi.DBAccess
{
    public class BorrowedRequestDBAccess
    {
        private readonly DBContext _context;

        public BorrowedRequestDBAccess(DBContext context)
        {
            _context = context;
        }

        public async Task<List<BorrowRequest>> GetAllBorrwRequests()
        {
            var borrowRequests = await _context.BorrowRequests.ToListAsync();

            return borrowRequests;
        }

        public async Task<BorrowRequest> GetBorrwRequest(int id)
        {
            var borrowRequests = await _context.BorrowRequests.FirstOrDefaultAsync(b => b.Id == id);

            return borrowRequests;
        }

        public async Task CreateBorrowRequest(BorrowRequest borrowRequest)
        {
            _context.BorrowRequests.Add(borrowRequest);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBorrowRequest(BorrowRequest borrowRequest)
        {
            _context.Entry(borrowRequest).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBorrowRequest(BorrowRequest borrowRequest)
        {
            _context.BorrowRequests.Remove(borrowRequest);

            await _context.SaveChangesAsync();
        }
    }
}
