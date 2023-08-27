using Avalonia.Controls;

namespace ElementsEditor
{
    public partial class FiltersListView : UserControl, IModalView
    {
        public FiltersListView(FiltersListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            lbx_filters.SelectionChanged += (s, a) =>
            {
                viewModel.UpdateCanExecuteChanged();
            };
        }

        public void OnApplying(ModalViewContext context)
        {
            
        }

        public void OnCanceling(ModalViewContext context)
        {
            
        }
    }
}
