using Online_Shop.Models;

namespace Online_Shop.Dtos
{
    public class PaymentViewModel
    {
        public Guid UserId { get; set; }
        public decimal Total { get; set; }
        public Guid SelectedAddress { get; set; }
        public Guid SelectedCard { get; set; }
        public List<CartItem> CartItems { get; set; } = new();
        public List<AddressDTO> Addresses { get; set; } = new();
        public List<CreditCard> CreditCards { get; set; } = new();
    }
}
