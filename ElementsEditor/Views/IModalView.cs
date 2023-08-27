using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor
{
    public interface IModalView
    {
        void OnApplying(ModalViewContext context);
        void OnCanceling(ModalViewContext context);
    }

    public class ModalViewContext
    {
        public object? Result { get; set; }                
    }
}
