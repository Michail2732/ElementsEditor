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
        private AccessRights _accessRights;
        private ElementState _previousState;

        public Element(string id, AccessRights accessRights)
        {
            Id = id;
            _accessRights = accessRights;            
        }

        internal IElementsStateWatcher? StateWatcher { get; set; }        
        
        public string Id { get; }
        
        public bool CanModify => _accessRights.HasFlag(AccessRights.Write);


        private ElementState _state;
        public ElementState State
        {
            get => _state;
            internal set
            {
                if (value >= _state)
                {
                    _previousState = _state;
                    _state = value;
                    _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
                }                    
            } 
        }

        internal void ResetState()
        {            
            _state = _previousState;
            _previousState = ElementState.None;
        }        

        #region INotifyPropertyChanged impl
        private event PropertyChangedEventHandler? _propertyChanged;        
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
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
            State = ElementState.Modified;
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
