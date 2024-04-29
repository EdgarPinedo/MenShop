using System.ComponentModel.DataAnnotations;

namespace Online_Shop.Dtos
{
    public class AddressDTO
    {
        public Guid Id { get; set; }

        public string Street { get; set; } = null!;

        public string City { get; set; } = null!;

        public string States { get; set; } = null!;

        public string Country { get; set; } = null!;

        [StringLength(5, MinimumLength = 5, ErrorMessage = "El código postal debe contener 5 números")]
        public string PostalCode { get; set; } = null!;

        public Guid UserId { get; set; }

        public string FullName { get; set; } = null!;
    }
}
