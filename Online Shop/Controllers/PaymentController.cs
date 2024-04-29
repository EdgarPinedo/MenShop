using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Dtos;
using Online_Shop.Services;
using System.Security.Claims;

namespace Online_Shop.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly AccountService _accountService;
        private readonly CartService _cartService;
        private readonly OrderService _orderService;

        public PaymentController(AccountService accountService, CartService cartService, OrderService orderService)
        {
            _accountService = accountService;
            _cartService = cartService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            PaymentViewModel viewModel = new();
            viewModel.UserId = userId;
            (viewModel.CartItems, viewModel.Total) = await _cartService.GetCart(userId);

            if(viewModel.CartItems.Count == 0) return RedirectToAction("Index", "Home");

            viewModel.Addresses = await _accountService.GetAddresses(userId);
            viewModel.CreditCards = await _accountService.GetCreditCards(userId);
            
            if (viewModel.Addresses.Count != 0)
                viewModel.SelectedAddress = viewModel.Addresses[0].Id;
            
            if (viewModel.CreditCards.Count != 0)
                viewModel.SelectedCard = viewModel.CreditCards[0].Id;
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PaymentViewModel viewModel)
        {
            if (viewModel.SelectedAddress == Guid.Empty || viewModel.SelectedCard == Guid.Empty)
            {
                ViewData["BadRequest"] = 
                    "Es necesario proporcionar una dirección de envío y un método de pago";
                return View(viewModel);
            }
            await _orderService.CreateOrder(viewModel);
            return RedirectToAction("OrdersHistory", "Account");
        }
    }
}
