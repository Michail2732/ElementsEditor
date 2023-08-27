using Avalonia.Controls;

namespace ElementsEditor
{
    public partial class AddNewElementView : UserControl, IModalView
    {
        public AddNewElementView(AddElementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public void OnApplying(ModalViewContext context)
        {
            context.Result = ((AddElementViewModel)DataContext!).ElementBuilder?.Build();
        }

        public void OnCanceling(ModalViewContext context)
        {            
        }
    }
}
