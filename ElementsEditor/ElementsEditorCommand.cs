using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElementsEditor
{
    public class ElementsEditorCommand : ICommand
    {
        private readonly ElementsEditorControl _elementsEditor;
        private Func<ElementsEditorControl, bool>? _canExecute;
        private Action<ElementsEditorControl> _execute;

        public ElementsEditorCommand(
            ElementsEditorControl elementsEditor, 
            Action<ElementsEditorControl> execute,
            Func<ElementsEditorControl, bool>? canExecute = null)
        {
            _elementsEditor = elementsEditor ?? throw new ArgumentNullException(nameof(elementsEditor));            
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null ? true : _canExecute(_elementsEditor);
        }

        public void Execute(object? parameter)
        {
            _execute.Invoke(_elementsEditor);
        }


        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(null, EventArgs.Empty);
        }

    }
}
