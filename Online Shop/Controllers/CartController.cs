using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Services;
using System.Security.Claims;

namespace Online_Shop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (cartItems, subtotal) = await _cartService.GetCart(userId);
            ViewData["subtotal"] = subtotal;
            return View(cartItems);
        }

        public async Task<IActionResult> AddToCart(Guid productId, int quantity, string color, string size)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _cartService.AddToCart(userId, productId, quantity, color, size);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> IncreaseQuantity(Guid itemId)
        {
            await _cartService.IncreaseQuantity(itemId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DecreaseQuantity(Guid itemId)
        {
            await _cartService.DecreaseQuantity(itemId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromCart(Guid itemId)
        {
            await _cartService.RemoveItemFromCart(itemId);
            return RedirectToAction("Index");
        }
    }
}
