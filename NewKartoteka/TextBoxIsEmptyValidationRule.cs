using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NewKartoteka
{
    class TextBoxIsEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string t = Convert.ToString(value);
            if (String.IsNullOrEmpty(t))
            {
                return new ValidationResult(false, "Заполните строку");
            }
            else
                return new ValidationResult(true, string.Empty);
        }
    }
}
