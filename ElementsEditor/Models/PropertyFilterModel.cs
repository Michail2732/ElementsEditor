using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElementsEditor
{

    public abstract class PropertyFilterModel<TProperty> : IPropertyFilterModel
    {
        protected readonly TryGetPropertyValueDelegate<TProperty> _propertyExtractor;
        private readonly ValueValidate<TProperty>? _valueValidator;

        private Logic _logic;
        public Logic SelectedLogic
        {
            get => _logic;
            set => SetAndRaisePropertyChanged(ref _logic, value);
        }        

        private ConditionOperation _operation;
        public ConditionOperation SelectedOperation
        {
            get => _operation;
            set => SetAndRaisePropertyChanged(ref _operation, value);
        }
        
        public string PropertyName { get; }
        public ConditionOperation[] Operations { get; }        
        public IEnumerable? Values { get; }
        public ValueType ValueType { get; }
        public Logic[] Logics { get; }


        private bool _isFirst;
        public bool IsFirst
        {
            get => _isFirst;
            set => SetAndRaisePropertyChanged(ref _isFirst, value);
        }


        private TProperty? _value;
        public TProperty? Value
        {
            get => _value;
            set
            {
                if (_valueValidator != null && !_valueValidator(value, out string? error))                
                    throw new ArgumentException(error);                
                SetAndRaisePropertyChanged(ref _value, value);
            }
        }

        public PropertyFilterModel(            
            string propertyName,
            ValueType valueType,
            ValueValidate<TProperty>? valueValidator,
            IEnumerable<TProperty>? awailableValues,
            TryGetPropertyValueDelegate<TProperty> propertyExtractor)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException($"\"{nameof(propertyName)}\" не может быть неопределенным или пустым.", nameof(propertyName));
            }

            _valueValidator = valueValidator;
            PropertyName = propertyName;
            Values = awailableValues;
            ValueType = valueType;
            Operations = GetAwailableOperations(valueType);
            _propertyExtractor = propertyExtractor;
            Logics = new[] { Logic.Or, Logic.And };
        }

        private ConditionOperation[] GetAwailableOperations(ValueType valueType)
        {
            switch (valueType)
            {
                case ValueType.String:
                    return new[] {ConditionOperation.Contains, ConditionOperation.EndWith, ConditionOperation.Equals,
                                  ConditionOperation.NotEquals, ConditionOperation.StartWith};
                case ValueType.Integer:
                case ValueType.Double:
                case ValueType.Decimal:
                case ValueType.DateTime:
                    return new[] {ConditionOperation.Large, ConditionOperation.Equals, ConditionOperation.NotEquals,
                                  ConditionOperation.LargeOrEquals, ConditionOperation.Less, ConditionOperation.LessOrEquals};
                case ValueType.Boolean:                
                    return new[] { ConditionOperation.Equals, ConditionOperation.NotEquals };
                default:
                    throw new Exception("Unknown value type");
            }
        }

        public abstract IPropertyFilter ToPropertyFilter();
        public object? GetValue() => Value;

        #region INotifyPropertyChanged impl
        public event PropertyChangedEventHandler? PropertyChanged;        

        protected void SetAndRaisePropertyChanged<T>(ref T oldValue, T newValue,
            [CallerMemberName] string property = "")
        {
            if (oldValue?.Equals(newValue) == true)
                return;
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }

    public sealed class StringPropertyFilterModel : PropertyFilterModel<string>
    {
        public StringPropertyFilterModel(string propertyName, ValueType valueType, 
            ValueValidate<string>? valueValidator, IEnumerable<string>? awailableValues, 
            TryGetPropertyValueDelegate<string> propertyExtractor) 
            : base(propertyName, valueType, valueValidator, awailableValues, propertyExtractor)
        {
        }

        public override IPropertyFilter ToPropertyFilter()
        {
            return new StringPropertyFilter(_propertyExtractor, PropertyName, Value, SelectedLogic, SelectedOperation);
        }        
    }

    public sealed class IntPropertyFilterModel : PropertyFilterModel<int>
    {
        public IntPropertyFilterModel(string propertyName, ValueType valueType,
            ValueValidate<int>? valueValidator, IEnumerable<int>? awailableValues,
            TryGetPropertyValueDelegate<int> propertyExtractor)
            : base(propertyName, valueType, valueValidator, awailableValues, propertyExtractor)
        {
        }

        public override IPropertyFilter ToPropertyFilter()
        {
            return new IntPropertyFilter(_propertyExtractor, PropertyName, Value, SelectedLogic, SelectedOperation);
        }
    }

    public sealed class DoublePropertyFilterModel : PropertyFilterModel<double>
    {
        public DoublePropertyFilterModel(string propertyName, ValueType valueType,
            ValueValidate<double>? valueValidator, IEnumerable<double>? awailableValues,
            TryGetPropertyValueDelegate<double> propertyExtractor)
            : base(propertyName, valueType, valueValidator, awailableValues, propertyExtractor)
        {
        }

        public override IPropertyFilter ToPropertyFilter()
        {
            return new DoublePropertyFilter(_propertyExtractor, PropertyName, Value, SelectedLogic, SelectedOperation);
        }
    }

    public sealed class DecimalPropertyFilterModel : PropertyFilterModel<decimal>
    {
        public DecimalPropertyFilterModel(string propertyName, ValueType valueType,
            ValueValidate<decimal>? valueValidator, IEnumerable<decimal>? awailableValues,
            TryGetPropertyValueDelegate<decimal> propertyExtractor)
            : base(propertyName, valueType, valueValidator, awailableValues, propertyExtractor)
        {
        }

        public override IPropertyFilter ToPropertyFilter()
        {
            return new DecimalPropertyFilter(_propertyExtractor, PropertyName, Value, SelectedLogic, SelectedOperation);
        }
    }


    public sealed class BooleanPropertyFilterModel : PropertyFilterModel<bool>
    {
        public BooleanPropertyFilterModel(string propertyName, ValueType valueType,
            ValueValidate<bool>? valueValidator, TryGetPropertyValueDelegate<bool> propertyExtractor)
            : base(propertyName, valueType, valueValidator, null, propertyExtractor)
        {
        }

        public override IPropertyFilter ToPropertyFilter()
        {
            return new BoolPropertyFilter(_propertyExtractor, PropertyName, Value, SelectedLogic, SelectedOperation);
        }
    }


    public sealed class DateTimePropertyFilterModel : PropertyFilterModel<DateTime>
    {
        public DateTimePropertyFilterModel(string propertyName, ValueType valueType,
            ValueValidate<DateTime>? valueValidator, IEnumerable<DateTime>? awailableValues,
            TryGetPropertyValueDelegate<DateTime> propertyExtractor)
            : base(propertyName, valueType, valueValidator, awailableValues, propertyExtractor)
        {
        }

        public override IPropertyFilter ToPropertyFilter()
        {
            return new DateTimePropertyFilter(_propertyExtractor, PropertyName, Value, SelectedLogic, SelectedOperation);
        }
    }
}
