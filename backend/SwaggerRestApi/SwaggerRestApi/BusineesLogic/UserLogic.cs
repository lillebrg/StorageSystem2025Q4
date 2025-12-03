using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SwaggerRestApi.DBAccess;
using SwaggerRestApi.Models;
using SwaggerRestApi.Models.DTO.Borrowed;
using SwaggerRestApi.Models.DTO.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SwaggerRestApi.BusineesLogic
{
    public class UserLogic
    {
        private readonly IConfiguration _configuration;
        private readonly UserDBAccess _userdbaccess;
        private readonly ItemDBAccess _itemdbaccess;
        private readonly SharedLogic _sharedlogic;

        public UserLogic(IConfiguration configuration, UserDBAccess userDBAccess, ItemDBAccess itemDBAccess, SharedLogic sharedlogic)
        {
            _configuration = configuration;
            _userdbaccess = userDBAccess;
            _itemdbaccess = itemDBAccess;
            _sharedlogic = sharedlogic;
        }

        /// <summary>
        /// Gets an user from an id
        /// </summary>
        /// <param name="id">The id of the user to be returned</param>
        /// <returns>User</returns>
        public async Task<ActionResult<UserGet>> GetUser(int id)
        {
            var imageBaseURL = _configuration["ImageUrl"];
            var user = await _userdbaccess.GetUser(id);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find the user" }); }

            var userReturn = new UserGet
            {
                email = user.Email,
                name = user.Name,
                borrowed_items = new List<BorrowedItems>(),
                change_password_on_next_login = user.ChangePasswordOnNextLogin,
                role = user.Role,
            };

            if (user.BorrowedItems.Count != 0)
            {
                foreach (var item in user.BorrowedItems)
                {
                    var specicItem = await _itemdbaccess.GetSpecificItemAndBaseItem(item);

                    if (specicItem.Id == 0) { break; }

                    BorrowedItems items = new BorrowedItems
                    {
                        specific_item_description = specicItem.Description,
                        specific_item_id = specicItem.Id,
                        base_item_id = specicItem.BaseItemId,
                        base_item_name = specicItem.BaseItem.Name,
                        base_item_picture = imageBaseURL + specicItem.BaseItem.Picture
                    };

                    if (items.base_item_picture == imageBaseURL) { items.base_item_picture = null; }

                    userReturn.borrowed_items.Add(items);
                }
            }

            return new OkObjectResult(userReturn);
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>A lits of all users</returns>
        public async Task<ActionResult<List<UsersDTO>>> GetAllUser()
        {
            var users = await _userdbaccess.GetAllUsers();
            List<UsersDTO> result = new List<UsersDTO>();

            foreach (var user in users)
            {
                UsersDTO userReturn = new UsersDTO
                {
                    id = user.Id,
                    email = user.Email,
                    name = user.Name,
                    role = user.Role,
                    borrowed_items = user.BorrowedItems.Count
                };
                result.Add(userReturn);
            }
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Changes the password of the user
        /// </summary>
        /// <param name="changePassword">Contains old/current password and new password</param>
        /// <param name="id">The id of the user that changes their password</param>
        /// <returns>True</returns>
        public async Task<ActionResult> ChangePassword(ChangePassword changePassword, int id)
        {
            var user = await _userdbaccess.GetUser(id);

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

            user.ChangePasswordOnNextLogin = false;

            await _userdbaccess.UpdateUser(user);

            return new OkObjectResult(true);
        }

        /// <summary>
        /// Resets password of an user
        /// </summary>
        /// <param name="changePassword">Contains a new password</param>
        /// <param name="id">The id of the user to be updated</param>
        /// <returns>True</returns>
        public async Task<ActionResult> ResetPassword(ResetPassword changePassword, int id)
        {
            var user = await _userdbaccess.GetUser(id);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find user" }); }

            if (!PasswordSecurity(changePassword.new_password))
            {
                return new BadRequestObjectResult(new { message = "Password does not match our security standard" });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(changePassword.new_password);

            user.Password = hashedPassword;

            user.ChangePasswordOnNextLogin = true;

            await _userdbaccess.UpdateUser(user);

            return new OkObjectResult(true);
        }

        /// <summary>
        /// Logs an user in
        /// </summary>
        /// <param name="login">Contain an email and password</param>
        /// <returns>Returns an authresponse that have a name, id, jwt token, role and whether they have to change password 
        /// on their nex login</returns>
        public async Task<ActionResult<AuthResponse>> Login(UserLogin login)
        {
            var user = await _userdbaccess.GetUserForLogin(login.email);

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
                role = user.Role,
                change_password_on_next_login = user.ChangePasswordOnNextLogin,
                refresh_token = await GenerateRefreshToken(user.Id)
            };

            return new OkObjectResult(authresponse);
        }

        /// <summary>
        /// Edits an user
        /// </summary>
        /// <param name="userUpdate">Contains an email and name</param>
        /// <param name="id">The id of the user to be edited</param>
        /// <returns>True</returns>
        public async Task<ActionResult> EditUser(UserUpdate userUpdate, int id)
        {
            var user = await _userdbaccess.GetUser(id);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find user" }); }

            if (userUpdate.name != user.Name && !string.IsNullOrEmpty(userUpdate.name))
            {
                user.Name = userUpdate.name;
            }

            if (userUpdate.email != user.Email && !string.IsNullOrEmpty(userUpdate.email))
            {
                if (EmailCheck(userUpdate.email))
                {
                    user.Email = userUpdate.email;
                }
            }

            await _userdbaccess.UpdateUser(user);

            return new OkObjectResult(true);
        }

        /// <summary>
        /// Edits an user admin only
        /// </summary>
        /// <param name="userUpdate">Contains an email, name, role and whether they have to change their password on next login</param>
        /// <param name="id">The id of the user to be edited</param>
        /// <returns>True</returns>
        public async Task<ActionResult> EditUser(UserUpdateAdmin userUpdate, int id)
        {
            var user = await _userdbaccess.GetUser(id);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find user" }); }

            if (userUpdate.name != user.Name && !string.IsNullOrEmpty(userUpdate.name))
            {
                user.Name = userUpdate.name;
            }

            if (userUpdate.email != user.Email && !string.IsNullOrEmpty(userUpdate.email))
            {
                if (EmailCheck(userUpdate.email))
                {
                    user.Email = userUpdate.email;
                }
            }

            if (userUpdate.role == "User" || userUpdate.role == "Operator" || userUpdate.role == "Admin")
            {
                user.Role = userUpdate.role;
            }

            user.ChangePasswordOnNextLogin = userUpdate.change_password_on_next_login;

            await _userdbaccess.UpdateUser(user);

            return new OkObjectResult(true);
        }

        /// <summary>
        /// Registers an user
        /// </summary>
        /// <param name="newUser">Contains an email, password, name and role</param>
        /// <returns>True</returns>
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

            if (await _userdbaccess.NameInUse(newUser.name))
            {
                return new BadRequestObjectResult(new { message = "Name is already in use" });
            }

            if (await _userdbaccess.NameInUse(newUser.email))
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
                    BorrowedItems = new List<int>(),
                    ChangePasswordOnNextLogin = true,
                    Role = newUser.role
                };

                await _userdbaccess.CreateUser(user);

                return new OkObjectResult(true);
            }

            return new BadRequestObjectResult(new { messeage = "Invalid role" });

        }

        /// <summary>
        /// Deletes an user
        /// </summary>
        /// <param name="id">The id of the user to be deleted</param>
        /// <returns>True</returns>
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _userdbaccess.GetUser(id);

            if (user == null || user.Id == 0) { return new NotFoundObjectResult(new { message = "Could not find the user" }); }

            await _userdbaccess.DeleteUser(user);

            return new OkObjectResult(true);
        }

        public async Task<ActionResult<List<UserBorroweGet>>> GetAllBorrowedItems(int id)
        {
            var imageBaseURL = _configuration["ImageUrl"];
            var borrowRequests = await _userdbaccess.GetAllBorrowRequest(id);

            List<UserBorroweGet> result = new List<UserBorroweGet>();

            foreach (var item in borrowRequests)
            {
                UserBorroweGet borrowGet = new UserBorroweGet
                {
                    accepted = item.Accepted,
                    id = item.Id,
                    base_item = new BaseItemFromBorrowed(),
                    specific_item = new SpecificItemFromBorrowed()
                };
                var specificItem = await _itemdbaccess.GetSpecificItem(item.SpecificItemId);
                var baseItem = await _itemdbaccess.GetBaseItem(specificItem.BaseItemId);

                borrowGet.base_item.id = baseItem.Id;
                borrowGet.base_item.name = baseItem.Name;
                borrowGet.base_item.description = baseItem.Description;
                borrowGet.base_item.image_url = imageBaseURL + baseItem.Picture;

                borrowGet.specific_item.id = specificItem.Id;
                borrowGet.specific_item.description = specificItem.Description;

                if (borrowGet.base_item.image_url == imageBaseURL) { borrowGet.base_item.image_url = null; }

                result.Add(borrowGet);
            }

            return new OkObjectResult(result);
        }
      
        public async Task<ActionResult<AuthResponse>> RefreshJWTToken(RefreshTokenRequest refreshToken)
        {
            var tokenEntity = await _userdbaccess.GetRefreshToken(refreshToken.refresh_token);

            if (tokenEntity == null) { return new NotFoundObjectResult(new { message = "Could not find token" }); }

            if (tokenEntity.ExpiresAt <= DateTime.Now)
            {
                return new BadRequestObjectResult(new { message = "Refresh token is not valid" });
            }

            var token = GenerateJwtToken(tokenEntity.User);

            var authresponse = new AuthResponse
            {
                name = tokenEntity.User.Name,
                email = tokenEntity.User.Email,
                access_token = token,
                role = tokenEntity.User.Role,
                change_password_on_next_login = tokenEntity.User.ChangePasswordOnNextLogin,
                refresh_token = await GenerateRefreshToken(tokenEntity.User.Id)
            };

            await _userdbaccess.DeleteRefreshToken(tokenEntity);

            return new OkObjectResult(authresponse);
        }

        public async Task<ActionResult> DeleteRefreshToken(RefreshTokenRequest refreshToken)
        {
            var tokenEntity = await _userdbaccess.GetRefreshToken(refreshToken.refresh_token);

            if (tokenEntity == null) { return new NotFoundObjectResult(new { message = "Could not find token" }); }

            await _userdbaccess.DeleteRefreshToken(tokenEntity);

            return new OkObjectResult(true);
        }

        // Our password security that is checked with regex
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

        // Genrerates an jwt token from our user object
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

        private async Task<string> GenerateRefreshToken(int userId)
        {
            var randomNumber = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshtoken = Convert.ToBase64String(randomNumber);

            RefreshToken token = new RefreshToken
            {
                Token = refreshtoken,
                UserId = userId,
                ExpiresAt = DateTime.Now.AddDays(7)            };

            return await _userdbaccess.AddRefreshToken(token);
        }
    }
}
