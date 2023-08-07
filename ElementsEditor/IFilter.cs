using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor
{
    public interface IFilter
    {
        Logic Logic { get; }        
        ValueType ValueType { get; }
        ConditionOperatioin Operation { get; }
        string PropertyName { get; }
        object? Value { get; }        
    }
}
