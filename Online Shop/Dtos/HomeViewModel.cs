namespace Online_Shop.Dtos;

public class HomeViewModel
{
    public List<ProductResponse> NewProducts { get; set; } = new();
    public List<ProductResponse> RecommendedProducts { get; set; } = new();
}
