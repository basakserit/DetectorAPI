using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace DetectorAPI
{
    public class UrlArrayValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var memberNames = new List<string>();
            try
            {
                var regex = new Regex(@"(http|https)://([\w\.\-]+)\.(com|net|edu|org)");
                var ips = value as string[];
                memberNames.AddRange((ips ?? throw new ArgumentException()).Where(ip => !regex.IsMatch(ip)));
                if (memberNames.Any()) return new ValidationResult(ErrorMessage, memberNames);
            }
            catch
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
