using System.Globalization;
using System.Windows.Controls;

namespace LaboratoryApp.src.Core.Rules
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult(false, "Vui lòng điền vào trường này");
            }
            return ValidationResult.ValidResult;
        }
    }
}
