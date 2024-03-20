using Avalonia.Controls;

namespace ElementsEditor
{
    public partial class AddNewElementView : UserControl
    {
        public AddNewElementView(AddElementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }        
    }
}
