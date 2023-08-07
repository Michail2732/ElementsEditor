using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor.Sample.Models
{    
	public class Filter<TProperty> : INotifyPropertyChanged
	{

		private string _name;
		public string Name
		{
			get => _name;
			set => SetAndRaisePropertyChanged(ref _name, value);
		}


		private TProperty _value;
		public TProperty Value
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
