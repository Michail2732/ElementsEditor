using Avalonia.Controls;

namespace ElementsEditor
{
    public partial class FiltersListView : UserControl
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
    }
}
