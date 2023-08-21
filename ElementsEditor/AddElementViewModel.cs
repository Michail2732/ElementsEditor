using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElementsEditor
{    

	public class AddElementViewModel : INotifyPropertyChanged
	{
        public IEnumerable<ElementBuilder> ElementBuilders { get; }


		private ElementBuilder? _elementBuilder;
		public ElementBuilder? ElementBuilder
		{
			get => _elementBuilder;
			set
			{
				if (value != null)
					value.ResetProperties();
                SetAndRaisePropertyChanged(ref _elementBuilder, value);
            }
		}		

        public AddElementViewModel(IEnumerable<ElementBuilder> elementBuilders)
        {
            ElementBuilders = elementBuilders ?? throw new ArgumentNullException(nameof(elementBuilders));
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
