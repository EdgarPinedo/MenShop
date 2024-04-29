using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Online_Shop.Dtos
{
    public class CreditCardDTO
    {
        public Guid Id { get; set; }

        [StringLength(16, MinimumLength = 16, ErrorMessage = "Debe contener 16 números")]
        public string Number { get; set; } = null!;

        public string ExpirationMonth { get; set; } = null!;

        public string ExpirationYear { get; set; } = null!;

        [RegularExpression("([0-9]+)", ErrorMessage = "Ingresa solo números")]
        [DataType(DataType.Password)]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Debe contener 3 números")]
        public string Cvv { get; set; } = null!;

        public Guid UserId { get; set; }

        public string FullName { get; set; } = null!;
    }
}
