using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace BraunauMobil.VeloBasar
{
    public class IbanAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var stringValue = value.ToString();
                if (!string.IsNullOrEmpty(stringValue))
                {
                    return IsValid(stringValue);
                }
            }
            return base.IsValid(value);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var stringValue = value.ToString();
                if (!string.IsNullOrEmpty(stringValue))
                {
                    if (!IsValid(stringValue))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }

        private const string _alphabet = "abcdefghijklmnopqrstuvwxyz";
        private static bool IsValid(string input)
        {
            //  https://de.wikipedia.org/wiki/Internationale_Bankkontonummer
            var trimmed = input.Replace(" ", "", StringComparison.InvariantCultureIgnoreCase);
            if (trimmed.Length < 15 || trimmed.Length > 30)
            {
                return false;
            }
            var normalized = trimmed.Substring(4, trimmed.Length - 4) + trimmed.Substring(0, 4);
            var numberString = new StringBuilder();
            foreach (var c in normalized)
            {
                if (char.IsDigit(c))
                {
                    numberString.Append(c);
                }
                else if (_alphabet.Contains(c, StringComparison.InvariantCultureIgnoreCase))
                {
                    numberString.Append($"{_alphabet.IndexOf(c, StringComparison.InvariantCultureIgnoreCase) + 10}");
                }
            }
            var number = BigInteger.Parse(numberString.ToString(), CultureInfo.InvariantCulture);
            return number % 97 == 1;
        }
    }
}
