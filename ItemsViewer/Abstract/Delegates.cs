using System;
using System.Collections.Generic;
using System.Text;

namespace ItemsViewer.Abstract
{
    public delegate bool TryGetPropertyValueDelegate<TResult>(Item element, out TResult result);

    public delegate bool ValueValidate<in Tproperty>(Tproperty? value, out string? error);
}
