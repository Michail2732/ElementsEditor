using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor
{
    public class WarningViewModel : DialogViewModel
    {

        private string _text;
        public string Text
        {
            get => _text;
            set => SetAndRaisePropertyChanged(ref _text, value);
        }


        public WarningViewModel(string text) : base(true)
        {
            _text = text;
        }
    }
}
