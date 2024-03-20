using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElementsEditor
{    
    public abstract class Element: INotifyPropertyChanged, IEquatable<Element?>
    {
        public string Id { get; }


        private bool _isModified;
        public bool IsModified
        {
            get => _isModified;
            internal set => SetAndRaisePropertyChanged(ref _isModified, value);
        }


        public Element(string id)
        {
            Id = id;            
        }                           
                

        #region INotifyPropertyChanged impl
        private event PropertyChangedEventHandler? _propertyChanged;        
        public event PropertyChangedEventHandler? PropertyChanged
        {
            add { _propertyChanged = (PropertyChangedEventHandler?)Delegate.Combine(_propertyChanged, value); }
            remove { _propertyChanged = (PropertyChangedEventHandler?)Delegate.Remove(_propertyChanged, value); }
        }

        
        protected void SetAndRaisePropertyChanged<T>(ref T oldValue, T newValue,
            [CallerMemberName] string property = "")
        {
            if (oldValue?.Equals(newValue) == true)
                return;            
            oldValue = newValue;            
            _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));            
        }        
        #endregion implement INotifyPropertyChanged

        #region IEquatable impl
        public override bool Equals(object? obj)
        {
            return Equals(obj as Element);
        }

        public bool Equals(Element? other)
        {
            return other is not null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
        }
        #endregion        
    }
}
