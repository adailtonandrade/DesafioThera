using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CompareNotEqualsAttribute : ValidationAttribute
    {
        public string _param1 { get; private set; }
        public string _param2 { get; private set; }
        public CompareNotEqualsAttribute(string param1, string param2)
        {
            _param1 = param1;
            _param2 = param2;
        }

        public string FormatErrorMessage(string name, string otherName)
        {
            return string.Format(ErrorMessageString, name, otherName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object obj = validationContext.ObjectInstance;
            var first = obj.GetType().GetProperty(_param1).GetValue(validationContext.ObjectInstance, null);
            var second = obj.GetType().GetProperty(_param2).GetValue(validationContext.ObjectInstance, null);
            if (first != null && second != null)
            {
                var otherProperty = obj.GetType().GetProperty(_param2);
                var displayName = (DisplayAttribute)Attribute.GetCustomAttribute(otherProperty, typeof(DisplayAttribute));
                if (first.Equals(second)) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName, displayName.GetName()));
            }
            return ValidationResult.Success;
        }
    }
}