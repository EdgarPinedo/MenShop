using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Models;
using Online_Shop.Services;
using System.Diagnostics;

namespace Online_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _productService;

        public HomeController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsHome();
            return View(products);
        }

        public async Task<IActionResult> Catalogue(int? page, string? category, string? brand, decimal? minPrice, decimal? maxPrice)
        {
            var products = await _productService.GetProducts(page, category, brand, minPrice, maxPrice);
            ViewData["title"] = _productService.CategoryToSpanish(category);
            ViewData["category"] = category;
            ViewData["brand"] = brand;
            ViewData["brands"] = await _productService.GetBrands(category);
            ViewData["minPrice"] = minPrice;
            ViewData["maxPrice"] = maxPrice;
            ViewData["pricerange"] = await _productService.GetPriceRange(category, brand);
            return View(products);
        }

        public async Task<IActionResult> ProductDetails(Guid id)
        {
            var product = await _productService.GetSingleProduct(id);
            return View(product);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}