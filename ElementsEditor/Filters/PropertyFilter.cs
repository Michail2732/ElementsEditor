using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElementsEditor
{	

    public abstract class PropertyFilter<TProperty> : IPropertyFilter, INotifyPropertyChanged
    {
		private readonly TryGetPropertyValueDelegate<TProperty> _propertyExtractor;
		private readonly Func<TProperty?, bool>? _valueValidator;

        public PropertyFilter(
            TryGetPropertyValueDelegate<TProperty> propertyExtractor,
            ValueType valueType,
            string filterName,
            TProperty? value = default,
            Logic logic = Logic.Or,
            ConditionOperatioin operation = ConditionOperatioin.Equals,
            Func<TProperty?, bool>? valueValidator = null,
            IEnumerable<TProperty>? awailableValues = null)
        {
            _propertyExtractor = propertyExtractor;
            _logic = logic;
            _valueType = valueType;
            _propertyName = filterName;
            _value = value;
            _operation = operation;
            _valueValidator = valueValidator;
            _awailableValues = awailableValues;
        }

        private Logic _logic;
		public Logic Logic
		{
			get => _logic;
			set => SetAndRaisePropertyChanged(ref _logic, value);
		}

		private IEnumerable<TProperty>? _awailableValues;
		public IEnumerable<TProperty>? AwailableValues
		{
			get => _awailableValues;
			set => SetAndRaisePropertyChanged(ref _awailableValues, value);
		}

		private ConditionOperatioin _operation;
		public ConditionOperatioin  Operation
		{
			get => _operation;
			set => SetAndRaisePropertyChanged(ref _operation, value);
		}

		private ValueType _valueType;
		public ValueType ValueType
		{
			get => _valueType;
			set => SetAndRaisePropertyChanged(ref _valueType, value);
		}

		private string? _propertyName;
		public string? PropertyName
		{
			get => _propertyName;
			set => SetAndRaisePropertyChanged(ref _propertyName, value);
		}


		private TProperty? _value;
		public TProperty? Value
		{
			get => _value;
			set 
			{
				if (_valueValidator == null || _valueValidator(value))
					SetAndRaisePropertyChanged(ref _value, value);
            } 
		}

        public object? GetValue()
        {
			return Value;
        }

        public bool TryGetPropertyValue(Element element, out TProperty? value)
        {
			return _propertyExtractor(element, out value);            
        }

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

		public abstract TResult Execute<TResult>(Element element, IPropertyFilterExecutor<TResult> executor);       
    }



}
