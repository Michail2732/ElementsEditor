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

    public class DelayUpdateBindingBehavior: Behavior<TextBox>
    {        
        private Timer _timer;

        public DelayUpdateBindingBehavior()
        {
            TextProperty.Changed.Subscribe(new Observer<AvaloniaPropertyChangedEventArgs>( e =>
                {
                    ((DelayUpdateBindingBehavior)e.Sender).OnBindingValueChanged();
                })
            );
            _timer = new Timer();
            _timer.AutoReset = false;
            _timer.Elapsed += OnTimerEllapsed;
        }

        public double DelayMilliseconds 
        {
            get => _timer.Interval;
            set => _timer.Interval = value; 
        }

        public static readonly StyledProperty<string?> TextProperty =
            AvaloniaProperty.Register<DelayUpdateBindingBehavior, string?>(
                nameof(Text));
        public string? Text
        {
            get { return GetValue(TextProperty); }
            set 
            {
                SetValue(TextProperty, value); 
            }
        }


        protected override void OnAttached()
        {                
            AssociatedObject!.AddHandler(TextBox.KeyDownEvent, KeyDown_Handler, handledEventsToo: true);
            AssociatedObject.AddHandler(TextBox.LostFocusEvent, LostFocus_Handler, handledEventsToo: true);
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject!.RemoveHandler(TextBox.KeyDownEvent, KeyDown_Handler);
            AssociatedObject.RemoveHandler(TextBox.LostFocusEvent, LostFocus_Handler);
            base.OnDetaching();
        }

        private void KeyDown_Handler(object sender, KeyEventArgs e)
        {
            _timer.Stop();            
            _timer.Start();    
        }

        private void LostFocus_Handler(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AssociatedObject!.Text))
                AssociatedObject!.Text = Text;
        }

        private void OnTimerEllapsed(object sender, ElapsedEventArgs e)
        {            
            Dispatcher.UIThread.Post(() =>
            {                
                if (!string.IsNullOrEmpty(AssociatedObject!.Text))
                    Text = AssociatedObject!.Text;                
            });                                        
        }

        private void OnBindingValueChanged()
        {
            if (AssociatedObject != null)
                AssociatedObject.Text = Text;
        }
    }
}
