using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "O campo E-mail não está em um formato válido.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }
        [JsonIgnore]
        [Display(Name = "Lembrar-Me")]
        public bool RememberMe { get; set; }
    }
}