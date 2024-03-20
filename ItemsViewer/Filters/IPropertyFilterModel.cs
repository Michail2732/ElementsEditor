using System.Collections;
using System.ComponentModel;

namespace ElementsEditor
{
    public interface IPropertyFilterModel: INotifyPropertyChanged
    {
        bool IsFirst { get; set; }
        Logic SelectedLogic { get; set; }
        Logic[] Logics { get; }
        ConditionOperation SelectedOperation { get; set; }
        ConditionOperation[] Operations { get; }
        string PropertyName { get; }        
        IEnumerable? Values { get; }        
        ValueType ValueType { get; }
        IPropertyFilter ToPropertyFilter();
        object? GetValue();
    }
}
