using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Services;
using System.Security.Claims;
using Online_Shop.Dtos;

namespace Online_Shop.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthService _authService;

        public AuthenticationController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claims = HttpContext.User;
            if (claims.Identity is not null && claims.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _authService.FindUserAsync(request);
            if (user is null)
            {
                ViewData["UnsuccessfulLogin"] = "El correo o contraseña son incorrectos";
                return View();
            }

            var (identity, properties) = _authService.ConfigClaims(user.Email, user.Id);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            ClaimsPrincipal claims = HttpContext.User;
            if (claims.Identity is not null && claims.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            bool userExist = await _authService.UserExistAsync(request.Email);
            if (userExist)
            {
                ViewData["EmailAlreadyRegistered"] = "Ya existe una cuenta con este email";
                return View();
            }

            var user = await _authService.RegisterUserAsync(request);
            if (user is not null)
            {
                var (identity, properties) = _authService.ConfigClaims(user.Email, user.Id);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
