using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Avalonia.Data;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using System.Timers;
using Avalonia.Input;
using Avalonia.Threading;

namespace ElementsEditor
{
    public class NumberBoxBehavior : Behavior<TextBox>
    {

        protected override void OnAttached()
        {
            AssociatedObject!.AddHandler(TextBox.TextInputEvent, TextInputEvent_Handler, RoutingStrategies.Tunnel, true);         
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject!.RemoveHandler(TextBox.TextInputEvent, TextInputEvent_Handler);            
            base.OnDetaching();
        }

        private void TextInputEvent_Handler(object sender, TextInputEventArgs e)
        {
            bool isAllGood = true;
            if (!string.IsNullOrEmpty(e.Text))
                for (int i = 0; i < e.Text!.Length; i++)
                {
                    if (e.Text[i] < '0' || e.Text[i] > '9')
                        isAllGood = false;
                }
            if (!isAllGood)
                e.Handled = true;
        }
    }
}
