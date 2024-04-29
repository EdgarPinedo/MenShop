using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Data;
using Online_Shop.Dtos;
using Online_Shop.Models;
using System.Security.Cryptography;
using System.Text;

namespace Online_Shop.Services
{
    public class AccountService
    {
        private readonly OnlineStoreContext _context;

        public AccountService(OnlineStoreContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUser(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ChangeNamesRequest?> GetNames(Guid id)
        {
            return await _context.Users.Select(u => new ChangeNamesRequest
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateNames(ChangeNamesRequest request)
        {
            await _context.Users.Where(u => u.Id == request.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.FirstName, request.FirstName)
                    .SetProperty(u => u.LastName, request.LastName)
                );
        }

        public async Task<ChangeEmailRequest?> GetEmail(Guid id)
        {
            return await _context.Users.Select(u => new ChangeEmailRequest
            {
                Id = u.Id,
                Email = u.Email
            }).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> IsEmailAlreadyRegistered(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task UpdateEmail(ChangeEmailRequest request)
        {
            await _context.Users.Where(u => u.Id == request.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.Email, request.Email)
                );
        }

        public async Task<bool> ComparePassword(Guid userId, string password)
        {
            return await _context.Users
                .AnyAsync(u => u.Id == userId && u.Password == GetSHA256(password));
        }

        public async Task UpdatePassword(ChangePasswordRequest request)
        {
            await _context.Users.Where(u => u.Id == request.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.Password, GetSHA256(request.NewPassword))
                );
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

        public async Task<List<AddressDTO>> GetAddresses(Guid userId)
        {
            return await _context.Addresses.Where(a => a.UserId == userId)
                .Select(a => new AddressDTO
                {
                    Id = a.Id,
                    Street = a.Street,
                    City = a.City,
                    States = a.States,
                    Country = a.Country,
                    PostalCode = a.PostalCode,
                    UserId = a.UserId,
                    FullName = a.FullName
                }).ToListAsync();
        }

        public async Task<AddressDTO?> GetAddress(Guid Id)
        {
            return await _context.Addresses.Select(a => new AddressDTO
                {
                    Id = a.Id,
                    Street = a.Street,
                    City = a.City,
                    States = a.States,
                    Country = a.Country,
                    PostalCode = a.PostalCode,
                    UserId = a.UserId,
                    FullName = a.FullName
                }).FirstOrDefaultAsync(a => a.Id == Id);
        }

        public async Task CreateAddress(AddressDTO address)
        {
            Address _address = new()
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City,
                States = address.States,
                Country = address.Country,
                PostalCode = address.PostalCode,
                UserId = address.UserId,
                FullName = address.FullName
            };

            await _context.Addresses.AddAsync(_address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAddress(AddressDTO address)
        {
            await _context.Addresses.Where(a => a.Id == address.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.FullName, address.FullName)
                    .SetProperty(a => a.Street, address.Street)
                    .SetProperty(a => a.City, address.City)
                    .SetProperty(a => a.States, address.States)
                    .SetProperty(a => a.Country, address.Country)
                    .SetProperty(a => a.PostalCode, address.PostalCode));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddress(Guid addressId)
        {
            await _context.Addresses.Where(a => a.Id == addressId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task<List<CreditCard>> GetCreditCards(Guid userId)
        {
            return await _context.CreditCards.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<CreditCardDTO?> GetSingleCreditCard(Guid Id)
        {
            return await _context.CreditCards.Select(a => new CreditCardDTO
            {
                Id = a.Id,
                Number = a.Number,
                ExpirationMonth = a.ExpirationMonth,
                ExpirationYear = a.ExpirationYear,
                Cvv = a.Cvv,
                UserId = a.UserId,
                FullName = a.FullName
            }).FirstOrDefaultAsync(a => a.Id == Id);
        }

        public async Task CreateCreditCard(CreditCardDTO card)
        {
            CreditCard _card = new()
            {
                Id = card.Id,
                Number = card.Number,
                ExpirationMonth = card.ExpirationMonth,
                ExpirationYear = card.ExpirationYear,
                Cvv = card.Cvv,
                UserId = card.UserId,
                FullName = card.FullName
            };

            await _context.CreditCards.AddAsync(_card);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCreditCard(CreditCardDTO card)
        {
            await _context.CreditCards.Where(a => a.Id == card.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.FullName, card.FullName)
                    .SetProperty(a => a.ExpirationMonth, card.ExpirationMonth)
                    .SetProperty(a => a.ExpirationYear, card.ExpirationYear));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCreditCard(Guid Id)
        {
            await _context.CreditCards.Where(a => a.Id == Id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrders(Guid userId)
        {
            return await _context.Orders
                .Where(a => a.UserId == userId)
                .Include(a => a.OrderProducts)
                .ThenInclude(o => o.Product)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }
    }
}
