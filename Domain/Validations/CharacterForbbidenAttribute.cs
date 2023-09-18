using System;
using System.ComponentModel.DataAnnotations;
using Domain.Util;

namespace Domain.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CharacterForbiddenAttribute : ValidationAttribute
    {
        public string Character { get { return DefaultRegex.General; } }

        public CharacterForbiddenAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && VerifyChars(value.ToString()))
            {
                return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }

        private bool VerifyChars(string pString)
        {
            return new Util.VerifyChar().ContainsInvalidCharacter(pString, Character);
        }
    }
}