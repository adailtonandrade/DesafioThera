using System.ComponentModel;

namespace Domain.Enum
{
    public enum ProfileEnum
    {
        [Description("Administrador")]
        Administrator = 1,

        [Description("Secretária")]
        Secretary = 2,

        [Description("Leitor")]
        Reader = 3,
    }
}