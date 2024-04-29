using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Online_Shop.Data;
using System.Security.Claims;
using System.Text;
using Online_Shop.Models;
using Online_Shop.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Online_Shop.Services
{
    public class AuthService
    {
        private readonly OnlineStoreContext _context;

        public AuthService(OnlineStoreContext context) { 
            _context = context;
        }

        public async Task<User?> FindUserAsync(LoginRequest request)
        {
            return await _context.Users.FirstOrDefaultAsync(
                u => u.Email == request.Email && u.Password == GetSHA256(request.Password));
        }

        public async Task<bool> UserExistAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> RegisterUserAsync(RegisterRequest request)
        {
            var user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = GetSHA256(request.Password)
            };

            var newUser = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return newUser.Entity;
        }

        public static string GetSHA256(string pass)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));

            var password = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                password.Append(bytes[i].ToString("x2"));
            }

            return password.ToString();
        }

        public (ClaimsPrincipal, AuthenticationProperties) ConfigClaims(string email, Guid id)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new()
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            return (new ClaimsPrincipal(identity), properties);
        }
    }
}
