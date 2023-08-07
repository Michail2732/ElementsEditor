using Avalonia.Controls;
using ElementsEditor.Sample.ViewModels;

namespace ElementsEditor.Sample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();            
        }
    }
}