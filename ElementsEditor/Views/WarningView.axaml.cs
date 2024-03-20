using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ElementsEditor
{
    public partial class WarningView : UserControl
    {
        public WarningView(WarningViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }        
    }
}