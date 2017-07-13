using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NewKartoteka
{
    class YearTextBoxValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string s = Convert.ToString(value);
            int t ;
            try
            {
                t = int.Parse(s);
            }
            catch
            {
                return new ValidationResult(false, "Неправильный формат числа");
            }
            if (t> DateTime.Now.Year)
            {
                return new ValidationResult(false, "Такой год еще не наступил");
            }
            else
                return new ValidationResult(true, string.Empty);
        }
    }
}
