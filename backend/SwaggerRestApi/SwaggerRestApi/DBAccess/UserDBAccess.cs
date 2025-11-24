using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwaggerRestApi.Models;

namespace SwaggerRestApi.DBAccess
{
    public class UserDBAccess
    {
        private readonly DBContext _context;

        public UserDBAccess(DBContext context)
        {
            _context = context;
        }

        public async Task CreateUser(User user)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUser(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // Updates the user in the database
        public async Task UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(User user)
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }

        // Searches for a user with the username
        public async Task<bool> NameInUse(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (user == null || user.Name == null || user.Name != name) { return false; }
            return true;
        }

        // Searches for a user with the email
        public async Task<bool> EmailInUse(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.Email == null || user.Email != email) { return false; }
            return true;
        }

        public async Task<User?> GetUserForLogin(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<string> AddRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<RefreshToken> GetRefreshToken(string refreshToken)
        {
            var refreshtoken = await _context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            return refreshtoken;
        }

        public async Task DeleteRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);

            await _context.SaveChangesAsync();
        }
    }
}
