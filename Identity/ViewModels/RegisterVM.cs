using Application.ViewModels;
using Domain.Validations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress(ErrorMessage = "O campo E-mail não está em um formato válido")]
        [StringLength(50, ErrorMessage = "O limite máximo de caracteres no campo {0} é de {1}")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(14, ErrorMessage = "O limite máximo de caracteres no campo {0} é de {1}")]
        [IsCpfValid(ErrorMessage = "Insira um CPF válido.")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required]
        [Display(Name = "Nome")]
        [StringLength(100, MinimumLength = 15, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
        [RegularExpression(@"^[A-zÀ-ÿ\s]*$", ErrorMessage = "No campo {0} não são permitidos caracteres especiais.")]
        public string Name { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "O limite máximo de caracteres no campo {0} é de {1}.")]
        [RegularExpression(@"^[A-zÀ-ÿ\s]*$", ErrorMessage = "No campo {0} não são permitidos caracteres especiais.")]
        [Display(Name = "Apelido")]
        public string NickName { get; set; }

        [Required]
        [Display(Name = "Perfil")]
        public int ProfileId { get; set; }
        [JsonIgnore]
        public string Profile { get; set; }
        [JsonIgnore]
        public List<ProfileVM> ProfileList { get; set; }
    }
}