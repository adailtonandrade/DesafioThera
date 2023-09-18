using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Enum
{
    public enum GenericStatusEnum
    {
        [Description("Inativo")]
        [Display(Name = "Inativo")]
        Inactive = 0,
        [Description("Ativo")]
        [Display(Name = "Ativo")]
        Active = 1
    }
}
