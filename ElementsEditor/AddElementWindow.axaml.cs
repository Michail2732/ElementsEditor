using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;

namespace ElementsEditor
{
    public partial class AddElementWindow : Window
    {        
        public AddElementWindow(
            AddElementViewModel viewModel,
            DataTemplates parentTemplates)
        {                        
            InitializeComponent();
            DataTemplates.AddRange(parentTemplates);
            DataContext = viewModel;
            btn_apply.Click += (s, a) =>
            {
                var viewModel = (AddElementViewModel)DataContext;
                var element = viewModel.ElementBuilder?.Build();
                Close(element);
            };
            btn_cancel.Click += (s, a) =>
            {
                Close(null);
            };            
        }
    }
}