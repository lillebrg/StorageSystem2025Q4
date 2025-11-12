using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace SwaggerRestApi.BusineesLogic
{
    public class UserLogic
    {
        private readonly IConfiguration _configuration;
        private readonly UserDBAccess _dbaccess;

        public UserLogic(IConfiguration configuration, UserDBAccess dBAccess)
        {
            _configuration = configuration;
            _dbaccess = dBAccess;
        }

        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _dbaccess.GetUser(id);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find the user" }); }

            var userReturn = new UserDto
            {
                email = user.Email,
                name = user.Name,
                borrowed_items = user.OnLoanItems,
                change_password_on_next_login = user.ChangePasswordOnNextLogin,
                roles = user.Role,
            };

            return new OkObjectResult(userReturn);
        }

        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            var users = await _dbaccess.GetAllUsers();

            return new OkObjectResult(users);
        }

        public async Task<ActionResult> ChangePassword(ChangePassword changePassword, int id)
        {
            var user = await _dbaccess.GetUser(id);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find user" }); }

            if (!BCrypt.Net.BCrypt.Verify(changePassword.current_password, user.Password))
            {
                return new BadRequestObjectResult(new { message = "Password did not match the old one" });
            }

            if (!PasswordSecurity(changePassword.new_password))
            {
                return new BadRequestObjectResult(new { message = "Password does not match our security standard" });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(changePassword.new_password);

            user.Password = hashedPassword;

            await _dbaccess.UpdateUser(user);

            return new OkObjectResult(true);
        }

        public async Task<ActionResult> ResetPassword(ResetPassword changePassword, int id)
        {
            var user = await _dbaccess.GetUser(id);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find user" }); }

            if (!PasswordSecurity(changePassword.new_password))
            {
                return new BadRequestObjectResult(new { message = "Password does not match our security standard" });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(changePassword.new_password);

            user.Password = hashedPassword;

            await _dbaccess.UpdateUser(user);

            return new OkObjectResult(true);
        }

        public async Task<ActionResult> Login(UserLogin login)
        {
            var user = await _dbaccess.GetUserForLogin(login.email);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find user" }); }

            if (!BCrypt.Net.BCrypt.Verify(login.password, user.Password))
            {
                return new BadRequestObjectResult(new { message = "Password is not correct" });
            }

            var token = GenerateJwtToken(user);

            var authresponse = new AuthResponse
            {
                name = user.Name,
                email = user.Email,
                access_token = token,
                role = user.Role
            };

            return new OkObjectResult(authresponse);
        }

        public async Task<ActionResult> EditUser(UserUpdate userUpdate, int id)
        {
            var user = await _dbaccess.GetUser(id);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find user" }); }

            if (userUpdate.name != user.Name && !string.IsNullOrEmpty(userUpdate.name))
            {
                user.Name = userUpdate.name;
            }

            if (userUpdate.email != user.Email && !string.IsNullOrEmpty(userUpdate.email))
            {
                if (!EmailCheck(userUpdate.email))
                {
                    user.Email = userUpdate.email;
                }
            }

            await _dbaccess.UpdateUser(user);

            return new OkObjectResult(true);
        }

        public async Task<ActionResult> EditUser(UserUpdateAdmin userUpdate, int id)
        {
            var user = await _dbaccess.GetUser(id);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find user" }); }

            if (userUpdate.name != user.Name && !string.IsNullOrEmpty(userUpdate.name))
            {
                user.Name = userUpdate.name;
            }

            if (userUpdate.email != user.Email && !string.IsNullOrEmpty(userUpdate.email))
            {
                if (!EmailCheck(userUpdate.email))
                {
                    user.Email = userUpdate.email;
                }
            }

            if (userUpdate.role == "User" || userUpdate.role == "Operator" || userUpdate.role == "Admin")
            {
                user.Role = userUpdate.role;
            }

            user.ChangePasswordOnNextLogin = userUpdate.change_password_on_next_login;

            await _dbaccess.UpdateUser(user);

            return new OkObjectResult(true);
        }

        public async Task<ActionResult> RegisterUser(UserCreate newUser)
        {
            if (!EmailCheck(newUser.email))
            {
                return new BadRequestObjectResult(new { message = "Invalid email address" });
            }

            if (!PasswordSecurity(newUser.password))
            {
                return new BadRequestObjectResult(new { message = "Password does not match our security standard" });
            }

            if (await _dbaccess.NameInUse(newUser.name))
            {
                return new BadRequestObjectResult(new { message = "Name is already in use" });
            }

            if (await _dbaccess.NameInUse(newUser.email))
            {
                return new BadRequestObjectResult(new { message = "Email is already in use" });
            }

            if (newUser.role == "User" || newUser.role == "Operator" || newUser.role == "Admin")
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.password);

                User user = new User
                {
                    Name = newUser.name,
                    Email = newUser.email,
                    Password = hashedPassword,
                    OnLoanItems = new List<int>(),
                    ChangePasswordOnNextLogin = true,
                    Role = newUser.role
                };

                await _dbaccess.CreateUser(user);

                return new OkObjectResult(true);
            }

            return new BadRequestObjectResult(new { messeage = "Invalid role" });

        }

        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _dbaccess.GetUser(id);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find the user" }); }

            await _dbaccess.DeleteUser(user);

            return new OkObjectResult(true);
        }

        private bool PasswordSecurity(string password)
        {
            var hasMinimum8Chars = new Regex(@".{8,}");

            return hasMinimum8Chars.IsMatch(password);
        }

        // Checks if the email has all the things an email should have
        private bool EmailCheck(string email)
        {
            return new Regex(@".+@.+\..+").IsMatch(email);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            (_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
