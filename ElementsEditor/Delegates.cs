using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor
{
    public delegate bool TryGetPropertyValueDelegate<TResult>(Element element, out TResult result);

    public delegate bool ValueValidate<in Tproperty>(Tproperty value, out string? error);
}
