using Domain.Validations;
using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels
{
    public class ChangePasswordVM
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        [CompareNotEquals("NewPassword", "OldPassword", ErrorMessage = "O campo {0} não pode ser igual ao campo {1}.")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Senha")]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}