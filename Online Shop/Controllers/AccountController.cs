using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Dtos;
using Online_Shop.Services;
using System.Security.Claims;

namespace Online_Shop.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoginAndSecurity()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _accountService.GetUser(userId);
            return View(user);
        }

        public async Task<IActionResult> UpdateNames()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var names = await _accountService.GetNames(userId);
            return View(names);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNames(ChangeNamesRequest request)
        {
            await _accountService.UpdateNames(request);
            return RedirectToAction("LoginAndSecurity");
        }


        public async Task<IActionResult> UpdateEmail()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var email = await _accountService.GetEmail(userId);
            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmail(ChangeEmailRequest request)
        {
            var isEmailAlreadyRegistered = await _accountService.IsEmailAlreadyRegistered(request.Email);
            if (isEmailAlreadyRegistered)
            {
                ViewData["BadEmail"] = "El email ya esta registrado";
                return View(request);
            }

            await _accountService.UpdateEmail(request);
            return RedirectToAction("LoginAndSecurity");
        }

        public IActionResult UpdatePassword()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            ChangePasswordRequest request = new() { Id = userId };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var isPasswordCorrect = await _accountService.ComparePassword(request.Id, request.Password);
            if (!isPasswordCorrect)
            {
                ViewData["WrongPassword"] = "La contraseña no es correcta";
                return View(request);
            }

            await _accountService.UpdatePassword(request);
            return RedirectToAction("LoginAndSecurity");
        }


        public async Task<IActionResult> Addresses()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var addresses = await _accountService.GetAddresses(userId);
            return View(addresses);
        }

        public IActionResult CreateAddress(string? redirect)
        {
            ViewData["redirect"] = redirect;
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            AddressDTO address = new() { Id = Guid.NewGuid(), UserId = userId };
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddress(AddressDTO address, string? redirect)
        {
            if (!ModelState.IsValid)
            {
                ViewData["redirect"] = redirect;
                return View(address);
            }
            await _accountService.CreateAddress(address);

            if (redirect != null)
                return RedirectToAction("Index", "Payment");
            
            return RedirectToAction("Addresses");
        }

        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            var address = await _accountService.GetAddress(id);
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress(AddressDTO address)
        {
            if (!ModelState.IsValid)
            {
                return View(address);
            }

            await _accountService.UpdateAddress(address);
            return RedirectToAction("Addresses");
        }

        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            await _accountService.DeleteAddress(id);
            return RedirectToAction("Addresses");
        }

        public async Task<IActionResult> Wallet()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var wallet = await _accountService.GetCreditCards(userId);
            return View(wallet);
        }

        public IActionResult CreateCreditCard(string? redirect)
        {
            ViewData["redirect"] = redirect;
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            CreditCardDTO card = new() { Id = Guid.NewGuid(), UserId = userId };
            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCreditCard(CreditCardDTO card, string? redirect)
        {
            if (!ModelState.IsValid)
            {
                ViewData["redirect"] = redirect;
                return View(card);
            }
            await _accountService.CreateCreditCard(card);

            if (redirect != null)
                return RedirectToAction("Index", "Payment");

            return RedirectToAction("Wallet");
        }

        public async Task<IActionResult> UpdateCreditCard(Guid id)
        {
            var card = await _accountService.GetSingleCreditCard(id);
            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCreditCard(CreditCardDTO card)
        {
            if (!ModelState.IsValid)
            {
                return View(card);
            }

            await _accountService.UpdateCreditCard(card);
            return RedirectToAction("Wallet");
        }

        public async Task<IActionResult> DeleteCreditCard(Guid id)
        {
            await _accountService.DeleteCreditCard(id);
            return RedirectToAction("Wallet");
        }

        public async Task<IActionResult> OrdersHistory()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var orders = await _accountService.GetOrders(userId);
            return View(orders);
        }
    }
}
