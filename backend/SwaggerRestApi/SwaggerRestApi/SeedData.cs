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

        public async Task StartData()
        {
            User newUser = new User
            {
                ChangePasswordOnNextLogin = false,
                Email = "admin",
                Name = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                Role = "Admin",
                OnLoanItems = new List<int>()
            };

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Role == "Admin");

            if (user == null || user.Id == 0) { _context.Users.Add(newUser); }

            await _context.SaveChangesAsync();
        }
    }
}
