using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor
{	
    public class ElementsEditorConfiguration : INotifyPropertyChanged
	{
        public ElementsEditorConfiguration(
            IElementsGateway elementsGateway,
            IEnumerable<ElementsFilter>? filters = null,
            int pageSize = -1,
            bool useAsync = false,
            bool useRemove = false)
        {
            _elementsGateway = elementsGateway ?? throw new ArgumentNullException(nameof(elementsGateway));
            _filters = new ObservableCollection<ElementsFilter>(filters ?? Array.Empty<ElementsFilter>());
            _usePaggination = pageSize > 0;
            _pageSize = pageSize;
            _useAsync = useAsync;
            _awailableRemove = useRemove;
        }		

        private IElementsGateway _elementsGateway;
		public IElementsGateway ElementsGateway
		{
			get => _elementsGateway;
			set => SetAndRaisePropertyChanged(ref _elementsGateway, value);
		}		


        private bool _usePaggination;
        public bool UsePaggination
        {
            get => _usePaggination;
            set => SetAndRaisePropertyChanged(ref _usePaggination, value);
        }


        private int _pageSize;
        public int PageSize
        {
            get => _pageSize;
            set => SetAndRaisePropertyChanged(ref _pageSize, value);
        }


		private bool _useAsync;
		public bool UseAsync
		{
			get => _useAsync;
			set => SetAndRaisePropertyChanged(ref _useAsync, value);
		}


		private bool _awailableRemove;        
        public bool AwailableRemove
		{
			get => _awailableRemove;
			set => SetAndRaisePropertyChanged(ref _awailableRemove, value);
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
