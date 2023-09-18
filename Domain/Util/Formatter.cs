namespace Domain.Util
{
    public class Formatter
    {
        public static string RemoveFormattingOfCnpjOrCpf(string cpfOrCnpj)
        {
            return cpfOrCnpj.Replace(".", "").Replace("-", "").Replace("_", "").Replace("/", "");
        }
    }
}