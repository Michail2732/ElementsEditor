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
        public ModalWIndow(DataTemplates templates,
            bool includeApplyButton = true,            
            bool includeCancelButton = true)
        {                        
            InitializeComponent();            
            DataTemplates.AddRange(templates);
            btn_apply.Click += (s, a) =>
            {
                if (View != null)
                {
                    var context = new ModalViewContext();
                    View.OnApplying(context);                    
                    Close(context.Result);
                }                
            };
            btn_cancel.Click += (s, a) =>
            {
                if (View != null)
                {
                    var context = new ModalViewContext();
                    View.OnCanceling(context);
                    Close(context.Result);
                }
            };
            if (!includeApplyButton) btn_apply.IsVisible = false;
            if (!includeCancelButton) btn_cancel.IsVisible = false;
        }

        public IModalView? View
        {
            get => cntn_content.Content as IModalView;
            set => cntn_content.Content = value;
        }

        private void HandleModalContext(ModalViewContext context)
        {
            if (context.Result != null)
                Close(context.Result);
        }
    }
}