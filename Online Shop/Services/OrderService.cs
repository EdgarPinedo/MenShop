using Microsoft.EntityFrameworkCore;
using Online_Shop.Data;
using Online_Shop.Dtos;
using Online_Shop.Models;

namespace Online_Shop.Services
{
    public class OrderService
    {
        private readonly OnlineStoreContext _context;

        public OrderService(OnlineStoreContext context)
        {
            _context = context;
        }

        public async Task CreateOrder(PaymentViewModel model)
        {
            Order order = new()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Total = model.Total,
                Status = "En revisión",
                UserId = model.UserId
            };

            List<OrderProduct> products = new();
            foreach(var item in model.CartItems)
            {
                products.Add(new OrderProduct()
                {
                    ProductId = item.ProductId,
                    OrderId = order.Id,
                    Quantity = item.Quantity
                });
            }

            order.OrderProducts = products;
            await _context.Orders.AddAsync(order);
            await _context.CartItems.Where(c => c.UserId == order.UserId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
    }
}
