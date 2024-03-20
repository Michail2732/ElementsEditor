using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;

namespace ElementsEditor
{
    public partial class ModalWIndow : Window
    {        
        public ModalWIndow()
        {
            
            InitializeComponent();                        
            btn_apply.Click += (s, a) => Close();
            btn_cancel.Click += (s, a) => Close();            
        }

        public object? View
        {
            get => cntn_content.Content; 
            set => cntn_content.Content = value;
        }
    }
}