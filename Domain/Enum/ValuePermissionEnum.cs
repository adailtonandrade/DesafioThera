using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Enum
{
    public enum ValuePermissionEnum
    {
        [Description("Criar")]
        [Display(Name = "Criar")]
        Create = 1,
        [Description("Atualizar")]
        [Display(Name = "Atualizar")]
        Update = 2,
        [Description("Consultar")]
        [Display(Name = "Consultar")]
        Consult = 3,
        [Description("Desativar")]
        [Display(Name = "Desativar")]
        Deactivate = 4
    }
}
