using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElementsEditor
{    
	internal class Filter : IFilter, INotifyPropertyChanged
    {
		private Logic _logic;
		public Logic Logic
		{
			get => _logic;
			set => SetAndRaisePropertyChanged(ref _logic, value);
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


		private object? _value;
		public object? Value
		{
			get => _value;
			set => SetAndRaisePropertyChanged(ref _value, value);
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
    }



}
