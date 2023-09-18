using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels
{
    public class ForgotVM
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}