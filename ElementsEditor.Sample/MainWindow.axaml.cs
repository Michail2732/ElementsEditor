using Avalonia.Controls;
using Avalonia.Media;
using ElementsEditor.Sample.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ElementsEditor.Sample
{
    public partial class MainWindow : Window
    {        
        public MainWindow(MainWindowViewModel viewModel)
        {            
            InitializeComponent();
            DataContext = viewModel;                        
        }
    }
}