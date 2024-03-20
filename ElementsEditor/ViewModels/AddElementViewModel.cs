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
				if (value != null)
					value.ResetProperties();
                SetAndRaisePropertyChanged(ref _elementBuilder, value);
            }
		}		

        public AddElementViewModel(IEnumerable<ElementBuilder> elementBuilders): base(true)
        {
            ElementBuilders = elementBuilders ?? throw new ArgumentNullException(nameof(elementBuilders));
        }

        protected override void ApplyCommandHandler(object? param)
        {
            
        }

    }

}
