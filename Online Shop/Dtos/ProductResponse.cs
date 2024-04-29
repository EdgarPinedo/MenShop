namespace Online_Shop.Dtos
{
    public class ProductResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string PathImage { get; set; } = null!;
    }
}
