using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using TradeSearchClient.ViewModel;

namespace TradeSearchClient.ValidationRules
{
    class RangeValidationRule : ValidationRule
    {
        public enum RuleType
        {
            Buy,Sell
        }
        public RuleType BuyOrSell { get; set; }
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var bindingGroup = value as BindingGroup;
            if (bindingGroup == null)
                return new ValidationResult(false,
                  "RangeValidationRule");

            return new ValidationResult(false, "Group");
        }
    }
}
