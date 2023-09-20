using System.Web;
using System.ComponentModel.DataAnnotations;
using Domain.Validations;
using System;
using Domain.Enum;
using Newtonsoft.Json;

namespace Application.ViewModels
{
    public class TalentVM
    {
        [Required]
        [Display(Name = "Nome")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
        [RegularExpression(@"^[A-zÀ-ÿ\s]*$", ErrorMessage = "No campo {0} não são permitidos caracteres especiais.")]
        public string FullName { get; set; }

        public int Id { get; set; }
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

        [Display(Name = "Situação")]
        public string Active { get; set; }

        [Display(Name = "Criado Em")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Atualizado Em")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Atualizado Por")]
        [JsonIgnore]
        public int UpdatedBy { get; set; }
        [Display(Name = "Currículo")]
        [JsonIgnore]
        public HttpPostedFileBase Resume { get; set; }
        [JsonIgnore]
        public byte[] ResumeFileData { get; set; }
        [Display(Name = "Currículo Atual")]
        public string ResumeFileName { get; set; }
        public string StatusName(string Active)
        {
            if (Active == ((int)GenericStatusEnum.Active).ToString())
                return "Ativo";
            return "Inativo";
        }
    }

    public class TalentDetailsVM : TalentVM
    {
        [Display(Name = "Atualizado Por")]
        new public string UpdatedBy { get; set; }
    }
}
