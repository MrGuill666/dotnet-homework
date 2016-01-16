using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TradeSearchClient.ValidationRules
{
    class OnlyPositiveNumbersRule: ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null) return new ValidationResult(true, null);
            try
            {
                if (((string)value).Length > 0)
                {
                    var number = Int32.Parse((String)value);
                    if (number < 0) throw new Exception();
                }
                return new ValidationResult(true, null);
            }catch(Exception e)
            {
                return new ValidationResult(false, "Only positive numbers accepted!");
            }
        }
    }
}
