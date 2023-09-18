using Domain.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class HasSpecialCharactersAttribute : ValidationAttribute
    {
        public string RegexString { get { return DefaultRegex.EmojiRegex; } }

        public HasSpecialCharactersAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string str)
            {
                if (String.IsNullOrEmpty(str)) return ValidationResult.Success;
                Regex rgx = new Regex(RegexString);
                Match match = rgx.Match(str);
                if (match.Success) return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
