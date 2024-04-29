using Microsoft.EntityFrameworkCore;
using Online_Shop.Data;
using Online_Shop.Dtos;
using Online_Shop.Models;
using System.Drawing;

namespace Online_Shop.Services
{
    public class CartService
    {
        private readonly OnlineStoreContext _context;

        public CartService(OnlineStoreContext context) 
        {
            _context = context;
        }

        public async Task AddToCart(Guid userId, Guid productId, int quantity, string color, string size)
        {
            var product = await IsProductInCart(productId, color, size);
            if (product != null)
            {
                product.Quantity += quantity;
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                CartItem cartItem = new()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    Color = color,
                    Size = size
                };
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CartItem?> IsProductInCart(Guid productId, string color, string size)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.Color == color && c.Size == size);
        }

        public async Task IncreaseQuantity(Guid itemId)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(i => i.Id == itemId);
            
            if(item != null)
            {
                item.Quantity++;
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DecreaseQuantity(Guid itemId)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(i => i.Id == itemId);

            if (item != null)
            {
                if(item.Quantity > 1)
                {
                    item.Quantity--;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    await RemoveItemFromCart(itemId);
                }
            }
        }

        public async Task<List<CartItem>> GetItemFromCart(Guid userId)
        {
            return await _context.CartItems.Where(c => c.UserId == userId).Include(c => c.Product).ToListAsync();
        }

        public async Task<(List<CartItem>, decimal)> GetCart(Guid userId)
        {
            decimal subtotal = 0;
            var items = await GetItemFromCart(userId);
            foreach(var item in items)
            {
                subtotal += item.Product.Price * item.Quantity;
            }
            return (items, subtotal);
        }

        public async Task RemoveItemFromCart(Guid productId)
        {
            await _context.CartItems.Where(p => p.Id == productId).ExecuteDeleteAsync();
        }
    }
}
