using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Enum
{
    public enum TypePermissionEnum
    {
        [Description("Gerenciamento de Talentos")]
        [Display(Name = "Gerenciamento de Talentos")]
        Talents = 1,
        [Description("Gerenciamento de Secretárias")]
        [Display(Name = "Gerenciamento de Secretárias")]
        Secretaries = 2,
        [Description("Gerenciamento de Administradores")]
        [Display(Name = "Gerenciamento de Administradores")]
        Administrators = 3,
        [Description("Gerenciamento de Leitores")]
        [Display(Name = "Gerenciamento de Leitores")]
        Readers = 4,
        [Description("Gerenciamento de Perfis")]
        [Display(Name = "Gerenciamento de Perfis")]
        Profiles = 5
    }
}
