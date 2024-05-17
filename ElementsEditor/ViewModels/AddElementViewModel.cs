using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ElementsEditor
{    

	public class AddElementViewModel : DialogViewModel
	{
        public IEnumerable<ElementBuilder> ElementBuilders { get; }


		private ElementBuilder? _elementBuilder;
		public ElementBuilder? ElementBuilder
		{
			get => _elementBuilder;
			set
			{
                if (_elementBuilder != null)
                    _elementBuilder.PropertyChanged -= ElementBuilderPropertyChanged;
                if (value != null)
					value.ResetProperties();
                SetAndRaisePropertyChanged(ref _elementBuilder, value);
                if (_elementBuilder != null)
                    _elementBuilder.PropertyChanged += ElementBuilderPropertyChanged;
            }
		}		

        public AddElementViewModel(IEnumerable<ElementBuilder> elementBuilders): base(true)
        {
            ElementBuilders = elementBuilders ?? throw new ArgumentNullException(nameof(elementBuilders));
        }        

        protected override bool CanApplyCommandHandler(object? param)
        {
            return _elementBuilder?.CanBuild ?? false;
        }

        private void ElementBuilderPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ElementBuilder.CanBuild))
                UpdateCanExecuteCommands();
        }
    }

}
