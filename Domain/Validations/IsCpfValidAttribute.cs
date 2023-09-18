using Domain.Util;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IsCpfValidAttribute : ValidationAttribute
    {
        private const int cpfLength = 11;

        public IsCpfValidAttribute()
        { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !ValidateCpf(value.ToString()))
                return new ValidationResult(ErrorMessage);
            return null;
        }
        private bool ValidateCpf(string cpf)
        {

            cpf = cpf.Trim();
            cpf = Formatter.RemoveFormattingOfCnpjOrCpf(cpf);
            if (cpf.Length == cpfLength)
            {
                var charDiff = cpf.Where(c => !c.Equals(cpf.First())).ToList();
                if (charDiff.Count == 0)
                    return false;
                int[] firstMultiplier = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] secondMultiplier = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                string tempCpf;
                string digit;
                int sum;
                int rest;
                tempCpf = cpf.Substring(0, 9);
                sum = 0;
                for (int i = 0; i < 9; i++)
                {
                    sum += int.Parse(tempCpf[i].ToString()) * firstMultiplier[i];
                }
                rest = sum % 11;
                if (rest < 2)
                {
                    rest = 0;
                }
                else
                {
                    rest = 11 - rest;
                }
                digit = rest.ToString();
                tempCpf = tempCpf + digit;
                sum = 0;
                for (int i = 0; i < 10; i++)
                {
                    sum += int.Parse(tempCpf[i].ToString()) * secondMultiplier[i];
                }
                rest = sum % 11;
                if (rest < 2)
                {
                    rest = 0;
                }
                else
                {
                    rest = 11 - rest;
                }
                digit = digit + rest.ToString();
                return cpf.EndsWith(digit);
            }
            return false;
        }
    }
}