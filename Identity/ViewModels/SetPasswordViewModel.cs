using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels
{
    public class SetPasswordVM
    {
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}