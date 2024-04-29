using System.ComponentModel.DataAnnotations;

namespace Online_Shop.Dtos
{
    public class RegisterRequest
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        [StringLength(15, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 15 caracteres")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "Los campos <Contraseña> y <Confirmar contraseña> deben ser iguales")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
