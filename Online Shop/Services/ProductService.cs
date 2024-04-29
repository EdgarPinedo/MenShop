using Microsoft.EntityFrameworkCore;
using Online_Shop.Data;
using Online_Shop.Dtos;
using Online_Shop.Models;
using Online_Shop.Utilities;

namespace Online_Shop.Services
{
    public class ProductService
    {
        private readonly OnlineStoreContext _context;

        public ProductService(OnlineStoreContext context)
        {
            _context = context;
        }

        public async Task<HomeViewModel> GetProductsHome()
        {
            _context.Database.EnsureCreated();
            return new HomeViewModel
            {
                NewProducts = await GetNewProducts(),
                RecommendedProducts = await GetRecommendedProducts()
            };
        }

        public async Task<List<ProductResponse>> GetNewProducts()
        {
            return await _context.Products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                PathImage = p.PathImage
            }).OrderByDescending(p => p.Price).Take(10).ToListAsync();
        }

        public async Task<List<ProductResponse>> GetRecommendedProducts()
        {
            return await _context.Products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                PathImage = p.PathImage
            }).OrderBy(p => Guid.NewGuid()).Take(14).ToListAsync();
        }

        public async Task<PaginatedList<ProductResponse>> GetProducts(int? page, string? category, string? brand, decimal? minPrice, decimal? maxPrice)
        {
            IQueryable<Product> query = _context.Products;

            if (category != null)
                query = query.Where(p => p.Category.Name == category);

            if (brand != null)
                query = query.Where(p => p.Name == brand);

            if(minPrice != null || maxPrice != null)
                query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);

            IQueryable<ProductResponse> productResponse = query.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                PathImage = p.PathImage
            }).OrderBy(p => p.Name);

            return await PaginatedList<ProductResponse>.Paginar(productResponse, page ?? 1, 9);
        }

        public async Task<List<BrandResponse>> GetBrands(string? category)
        {
            IQueryable<Product> brands = _context.Products;

            if (category != null)
                brands = brands.Where(p => p.Category.Name == category);

            return await brands.GroupBy(g => g.Name).Select(g => new BrandResponse(
                g.Key,
                g.Count()
            )).ToListAsync();
        }

        public async Task<PriceRangeResponse> GetPriceRange(string? category, string? brandName)
        {
            decimal min;
            decimal max;
            IQueryable<Product> query = _context.Products;
            
            if (category != null)
                query = query.Where(p => p.Category.Name == category);

            if (brandName != null)
                query = query.Where(p => p.Name == brandName);

            min = await query.MinAsync(p => p.Price);
            max = await query.MaxAsync(p => p.Price);

            return new PriceRangeResponse(min, max);
        }

        public async Task<ProductResponse?> GetSingleProduct(Guid id)
        {
            return await _context.Products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                PathImage = p.PathImage
            }).FirstOrDefaultAsync(p => p.Id == id);
        }

        public string? CategoryToSpanish(string? category)
        {
            if (category == "Shirts")
            {
                return "Camisas";
            }

            if (category == "Pants")
            {
                return "Pantalones";
            }

            if (category == "Shoes")
            {
                return "Calzado";
            }

            if (category == "Sweaters")
            {
                return "Sueteres";
            }

            return null;
        }
    }
}
