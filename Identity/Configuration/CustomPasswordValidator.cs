using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Identity.Configuration
{
    public class CustomPasswordValidator : IIdentityValidator<string>
    {
        private string DigitMessage = "A senha deve possuir números (0-9).";
        private string LowercaseMessage = "A senha deve possuir letras minúsculas (a-z).";
        private string UppercaseMessage = "A senha deve possuir letras maiúsculas (A-Z).";
        private string NonLetterOrDigitMessage = "A senha deve possuir caracteres especiais (!,#,@,à).";
        private string WhiteSpaceMessage = "A senha não deve conter espaços em branco.";

        private int PasswordLength { get; set; }

        private const string DigitPattern = @"[0-9]+";
        private const string LowercasePattern = @"[a-z]+";
        private const string UppercasePattern = @"[A-Z]+";
        private const string NonLetterOrDigitPattern = @"[!@#$%&*]+";

        public bool RequireDigit { get; set; }
        public int RequiredLength { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireNonLetterOrDigit { get; set; }
        public bool RequireUppercase { get; set; }


        public CustomPasswordValidator() { }

        public Task<IdentityResult> ValidateAsync(string item)
        {
            List<string> _errors = new List<string>();
            if (!VerifyDigit(item)) _errors.Add(DigitMessage);
            if (!VerifyLength(item)) _errors.Add("A senha deve possuir no mínimo " + RequiredLength + "dígitos");
            if (!VerifyLowerCase(item)) _errors.Add(LowercaseMessage);
            if (!VerifyUpperCase(item)) _errors.Add(UppercaseMessage);
            if (!VerifyWhiteSpace(item)) _errors.Add(WhiteSpaceMessage);
            if (PasswordLength == item.Length) _errors.Add(NonLetterOrDigitMessage);
            if (_errors.Count() > 0)
            {
                return Task.FromResult(IdentityResult.Failed(_errors.ToArray()));
            }
            else
            {
                return Task.FromResult(IdentityResult.Success);
            }
        }

        private bool VerifyDigit(string item)
        {
            if (RequireDigit)
            {
                int length = Regex.Match(item, DigitPattern).Length;
                PasswordLength += length;
                if (length == 0)
                    return false;
            }
            return true;
        }

        private bool VerifyLowerCase(string item)
        {
            if (RequireLowercase)
            {
                int length = Regex.Match(item, LowercasePattern).Length;
                PasswordLength += length;
                if (length == 0)
                    return false;
            }
            return true;
        }

        private bool VerifyUpperCase(string item)
        {
            if (RequireUppercase)
            {
                int length = Regex.Match(item, UppercasePattern).Length;
                PasswordLength += length;
                if (length == 0)
                    return false;
            }
            return true;
        }

        private bool VerifyLength(string item)
        {
            if (RequiredLength > 0)
            {
                if (item.Count() < RequiredLength)
                    return false;
            }
            return true;
        }

        private bool VerifyWhiteSpace(string item)
        {
            int length = item.Count(i => i == ' ');
            PasswordLength += length;
            if (length > 0)
                return false;
            return true;
        }
    }
}
