using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElementsEditor
{   
	public abstract class ElementBuilder : INotifyPropertyChanged
	{
		public string Name { get; }

        protected ElementBuilder(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public abstract Element Build();
		public abstract void ResetProperties();

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
