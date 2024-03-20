using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ItemsViewer.Filters
{    
    public sealed class PropertyFilterDescriptor
    {
        public string PropertyName { get; }
        public readonly ValueType ValueType;
        public readonly Delegate PropertyExtractor;
        public readonly Delegate? ValueValidator;
        public readonly IEnumerable? AwailableValues;

        public PropertyFilterDescriptor(string propertyName, 
            TryGetPropertyValueDelegate<string> stringValueExtracter, 
            ValueValidate<string>? valueValidator = null,
            IEnumerable<string>? awailableValues = null)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.String;
            AwailableValues = awailableValues;
        }

        public PropertyFilterDescriptor(string propertyName,
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

        public PropertyFilterDescriptor(string propertyName,
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

        public PropertyFilterDescriptor(string propertyName,
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

        public PropertyFilterDescriptor(string propertyName,
            TryGetPropertyValueDelegate<bool> stringValueExtracter, 
            ValueValidate<bool>? valueValidator = null)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyExtractor = stringValueExtracter ?? throw new ArgumentNullException(nameof(stringValueExtracter));
            ValueType = ValueType.Boolean;
            ValueValidator = valueValidator;
        }

        public PropertyFilterDescriptor(string propertyName,
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

        public PropertyFilterDescriptor(string propertyName,
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

        internal IPropertyFilterModel CreateFilterModel(bool isFirst)
        {
            switch (ValueType)
            {
                case ValueType.String:
                    return new StringPropertyFilterModel(
                        PropertyName,
                        ValueType.String,
                        ValueValidator as ValueValidate<string>,
                        AwailableValues as IEnumerable<string>,
                        (TryGetPropertyValueDelegate<string>)PropertyExtractor)
                    { IsFirst = isFirst};
                case ValueType.Integer:
                    return new IntPropertyFilterModel(
                        PropertyName,
                        ValueType.Integer,
                        ValueValidator as ValueValidate<int>,
                        AwailableValues as IEnumerable<int>,
                        (TryGetPropertyValueDelegate<int>)PropertyExtractor)
                    { IsFirst = isFirst };
                case ValueType.Double:
                    return new DoublePropertyFilterModel(
                        PropertyName,
                        ValueType.Double,
                        ValueValidator as ValueValidate<double>,
                        AwailableValues as IEnumerable<double>,
                        (TryGetPropertyValueDelegate<double>)PropertyExtractor)
                    { IsFirst = isFirst };
                case ValueType.Decimal:
                    return new DecimalPropertyFilterModel(
                        PropertyName,
                        ValueType.Decimal,
                        ValueValidator as ValueValidate<decimal>,
                        AwailableValues as IEnumerable<decimal>,
                        (TryGetPropertyValueDelegate<decimal>)PropertyExtractor)
                    { IsFirst = isFirst };
                case ValueType.Boolean:
                    return new BooleanPropertyFilterModel(
                        PropertyName,
                        ValueType.Boolean,
                        ValueValidator as ValueValidate<bool>,                        
                        (TryGetPropertyValueDelegate<bool>)PropertyExtractor)
                    { IsFirst = isFirst };
                case ValueType.DateTime:
                    return new DateTimePropertyFilterModel(
                        PropertyName,
                        ValueType.DateTime,
                        ValueValidator as ValueValidate<DateTime>,
                        AwailableValues as IEnumerable<DateTime>,
                        (TryGetPropertyValueDelegate<DateTime>)PropertyExtractor)
                    { IsFirst = isFirst };
                default:
                    throw new Exception("Unknown value type");
            }
        }
    }
}
