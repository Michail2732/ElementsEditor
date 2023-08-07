using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor
{
    public delegate bool ValueValidate<in Tproperty>(Tproperty value, out string? error);

    public sealed class FilterInfo
    {
        public readonly string Name;
        public readonly ValueType ValueType;
        public readonly Delegate PropertyExtractor;
        public readonly Delegate? ValueValidator;

        public FilterInfo(string propertyName, Func<Element, string> stringValueExtracter, ValueValidate<string>? valueValidator = null)
        {
            Name = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.String;
        }

        public FilterInfo(string propertyName, Func<Element, int> stringValueExtracter, ValueValidate<int>? valueValidator = null)
        {
            Name = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.Integer;
            ValueValidator = valueValidator;
        }

        public FilterInfo(string propertyName, Func<Element, double> stringValueExtracter, ValueValidate<double>? valueValidator = null)
        {
            Name = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.Double;
            ValueValidator = valueValidator;
        }

        public FilterInfo(string propertyName, Func<Element, Decimal> stringValueExtracter, ValueValidate<decimal>? valueValidator = null)
        {
            Name = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.Decimal;
            ValueValidator = valueValidator;
        }

        public FilterInfo(string propertyName, Func<Element, bool> stringValueExtracter, ValueValidate<bool>? valueValidator = null)
        {
            Name = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.Boolean;
            ValueValidator = valueValidator;
        }

        public FilterInfo(string propertyName, Func<Element, DateTime> stringValueExtracter, ValueValidate<DateTime>? valueValidator = null)
        {
            Name = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.DateTime;
            ValueValidator = valueValidator;
        }
    }
}
