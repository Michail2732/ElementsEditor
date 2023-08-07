using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor.Filters
{    

    internal interface IFilter<TProperty>
    {
        string PropertyName { get; }
        Logic Logic { get; }
        ConditionOperatioin Operation { get; }
        TProperty Value { get; }
        TProperty GetValue(Element element);
    }
}
