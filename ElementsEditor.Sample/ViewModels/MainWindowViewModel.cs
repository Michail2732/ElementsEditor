using ElementsEditor.Sample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ElementsEditor.Sample.ViewModels
{ 
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		public List<object> Filters { get; }

		public MainWindowViewModel()
		{
			Filters = new List<object>()
			{
				new Filter<string>{ Name="String filter", Value="Hello"},
				new Filter<int>{ Name="Int32 filter", Value=124},
				new Filter<bool>{ Name="Boolean filter", Value=true},
				new Filter<DateTimeOffset>{ Name="DateTimeOffset filter", Value=DateTimeOffset.Now},
			};
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
