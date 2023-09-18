using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress(ErrorMessage = "O campo E-mail não está em um formato válido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
