using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewKartoteka
{
    class ValidCurrentYear: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int _currentYear;
            int.TryParse(value.ToString(), out _currentYear);
            if (_currentYear < DateTime.Now.Year)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
