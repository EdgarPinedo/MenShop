using System.ComponentModel.DataAnnotations;

namespace Online_Shop.Dtos
{
    public class ChangePasswordRequest
    {
        public Guid Id { get; set; }
        public string Password { get; set; } = null!;

        [StringLength(15, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 15 caracteres")]
        public string NewPassword { get; set; } = null!;

        [Compare("NewPassword", ErrorMessage = "Los campos <Tu contraseña nueva> y <Repite la contraseña nueva> deben ser iguales")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
