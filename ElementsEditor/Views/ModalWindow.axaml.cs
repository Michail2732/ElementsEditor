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
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            if (change.Property.Name == nameof(DataContext))
            {
                var vm = change.OldValue as DialogViewModel;
                if (vm != null)
                    vm.Close -= CloseWindowHandler;
                vm = change.NewValue as DialogViewModel;
                if (vm != null)
                    vm.Close += CloseWindowHandler;
            }
        }

        public object? View
        {
            get => cntn_content.Content; 
            set => cntn_content.Content = value;
        }

        private void CloseWindowHandler(object sender, EventArgs e)
        {
            Close();
        }
    }
}