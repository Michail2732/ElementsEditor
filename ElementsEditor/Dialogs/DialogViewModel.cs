using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor
{    
    public abstract class DialogViewModel: INotifyPropertyChanged
    {
        public Command ApplyCommand { get; }
        public Command CancelCommand { get; }
        public bool IsVisibleCancel { get; }

        public Result Result { get; protected set; }

        protected DialogViewModel(bool isVisibleCancel)
        {
            IsVisibleCancel = isVisibleCancel;
            ApplyCommand = new Command(ApplyCommandHandler);
            CancelCommand = new Command(CancelCommandHandler);
        }

        protected virtual void ApplyCommandHandler(object? param)
        {
            Result = Result.Ok;
        }

        protected virtual void CancelCommandHandler(object? param)
        {
            Result = Result.Cancel;
        }

        #region INotifyPropertyChanged impl
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void SetAndRaisePropertyChanged<T>(ref T oldValue, T newValue,
            [CallerMemberName] string property = "")
        {
            if (oldValue?.Equals(newValue) == true)
                return;
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
