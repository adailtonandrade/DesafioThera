using Domain.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class ProfileVM
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "O limite máximo de caracteres no campo {0} é de {1}.")]
        [RegularExpression(@"^[A-zÀ-ÿ0-9\s]*$", ErrorMessage = "No campo {0} não são permitidos caracteres especiais.")]
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name = "Ativo")]
        public string Active { get; set; }
        [Display(Name = "Permissões")]
        public List<List<PermissionVM>> PermissionGrouped { get; set; }
        public List<PermissionVM> PermissionList { get; set; }
        public virtual ICollection<ProfilePermissionVM> ProfilePermissions { get; set; }
        public virtual ICollection<UserVM> UserList { get; set; }

        [Required(ErrorMessage = "Selecione pelo menos um item.")]
        public List<int> SelectedPermissionIdList { get; set; }
        public string StatusName(string Active)
        {
            if (Active == ((int)GenericStatusEnum.Active).ToString()) return "Ativo";
            return "Inativo";
        }
    }

    public class ProfileViewIndex
    {
        public string ResearchName { get; set; }
        public int? ResearchActive { get; set; }
        public List<ProfileVM> ProfileList { get; set; }
        public string StatusName(string Active)
        {
            if (Active == ((int)GenericStatusEnum.Active).ToString()) return "Ativo";
            return "Inativo";
        }
    }
}
