using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor
{    
    public sealed class PropertyFilterCreator
    {
        public readonly string PropertyName;
        public readonly ValueType ValueType;
        public readonly Delegate PropertyExtractor;
        public readonly Delegate? ValueValidator;
        public readonly IEnumerable? AwailableValues;

        public PropertyFilterCreator(string propertyName, 
            TryGetPropertyValueDelegate<string> stringValueExtracter, 
            ValueValidate<string>? valueValidator = null,
            IEnumerable<string>? awailableValues = null)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.String;
            AwailableValues = awailableValues;
        }

        public PropertyFilterCreator(string propertyName,
            TryGetPropertyValueDelegate<int> stringValueExtracter, 
            ValueValidate<int>? valueValidator = null,
            IEnumerable<int>? awailableValues = null)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.Integer;
            ValueValidator = valueValidator;
            AwailableValues = awailableValues;
        }

        public PropertyFilterCreator(string propertyName,
            TryGetPropertyValueDelegate<double> stringValueExtracter, 
            ValueValidate<double>? valueValidator = null,
            IEnumerable<double>? awailableValues = null)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.Double;
            ValueValidator = valueValidator;
            AwailableValues = awailableValues;
        }

        public PropertyFilterCreator(string propertyName,
            TryGetPropertyValueDelegate<decimal> stringValueExtracter,
            ValueValidate<decimal>? valueValidator = null,
            IEnumerable<decimal>? awailableValues = null)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.Decimal;
            ValueValidator = valueValidator;
            AwailableValues = awailableValues;
        }

        public PropertyFilterCreator(string propertyName,
            TryGetPropertyValueDelegate<bool> stringValueExtracter, 
            ValueValidate<bool>? valueValidator = null)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.Boolean;
            ValueValidator = valueValidator;
        }

        public PropertyFilterCreator(string propertyName,
            TryGetPropertyValueDelegate<DateTime> stringValueExtracter,
            ValueValidate<DateTime>? valueValidator = null,
            IEnumerable<DateTime>? awailableValues = null)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.DateTime;
            ValueValidator = valueValidator;
            AwailableValues = awailableValues;
        }

        public PropertyFilterCreator(string propertyName,
            TryGetPropertyValueDelegate<object> stringValueExtracter, 
            ValueValidate<object>? valueValidator = null,
            IEnumerable<object>? awailableValues = null)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.DateTime;
            ValueValidator = valueValidator;
            AwailableValues = awailableValues;
        }

        public IPropertyFilter Create()
        {
            switch (ValueType)
            {
                case ValueType.String:
                    return new StringPropertyFilter(
                        (TryGetPropertyValueDelegate<string>)PropertyExtractor, 
                        PropertyName, 
                        valueValidator: ValueValidator as Func<string?, bool>,
                        awailableValues: AwailableValues as IEnumerable<string>);
                case ValueType.Integer:
                    return new IntPropertyFilter(
                        (TryGetPropertyValueDelegate<int>)PropertyExtractor,
                        PropertyName,
                        valueValidator: ValueValidator as Func<int, bool>,
                        awailableValues: AwailableValues as IEnumerable<int>);
                case ValueType.Double:
                    return new DoublePropertyFilter(
                        (TryGetPropertyValueDelegate<double>)PropertyExtractor,
                        PropertyName,
                        valueValidator: ValueValidator as Func<double, bool>,
                        awailableValues: AwailableValues as IEnumerable<double>);
                case ValueType.Decimal:
                    return new DecimalPropertyFilter(
                        (TryGetPropertyValueDelegate<decimal>)PropertyExtractor,
                        PropertyName,
                        valueValidator: ValueValidator as Func<decimal, bool>,
                        awailableValues: AwailableValues as IEnumerable<decimal>);
                case ValueType.Boolean:
                    return new BoolPropertyFilter(
                        (TryGetPropertyValueDelegate<bool>)PropertyExtractor,
                        PropertyName,
                        valueValidator: ValueValidator as Func<bool, bool>);
                case ValueType.DateTime:
                    return new DateTimePropertyFilter(
                        (TryGetPropertyValueDelegate<DateTime>)PropertyExtractor,
                        PropertyName,
                        valueValidator: ValueValidator as Func<DateTime, bool>,
                        awailableValues: AwailableValues as IEnumerable<DateTime>);
                case ValueType.Custom:
                    return new CustomPropertyFilter(
                        (TryGetPropertyValueDelegate<object>)PropertyExtractor,
                        PropertyName,
                        valueValidator: ValueValidator as Func<object?, bool>,
                        awailableValues: AwailableValues as IEnumerable<object>);
                default:
                    throw new Exception("Unknown value type");
            }
        }
    }
}
