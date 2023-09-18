using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        [EmailAddress(ErrorMessage = "O campo E-mail não está em um formato válido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6,  ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}