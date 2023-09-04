using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor
{
    public class PropertyFilterToFuncConverter<TElement> : IPropertyFilterConverter<Func<TElement, bool>>
        where TElement : Element
    {
        public Func<TElement, bool> Convert(IReadOnlyList<IPropertyFilter> filters)
        {
            if (filters.Count == 0)
                return element => true;
            return element =>
            {
                bool isSuccess = filters[0].Convert(this)(element);
                for (int i = 1; i < filters.Count; i++)
                {
                    bool localRes = filters[i].Convert(this)(element);
                    if (filters[i].Logic == Logic.Or && isSuccess)
                        return true;
                    else if (filters[i].Logic == Logic.Or)
                        isSuccess = localRes;
                    else
                        isSuccess &= localRes;
                }
                return isSuccess;
            };
        }

        public Func<TElement, bool> Convert(StringPropertyFilter filter)
        {
            return element =>
            {
                if (filter.TryGetPropertyValue(element, out var propertValue))
                    return filter.Operation.OperationExecute(propertValue, filter.Value);
                return false;
            };
        }

        public Func<TElement, bool> Convert(IntPropertyFilter filter)
        {
            return element =>
            {
                if (filter.TryGetPropertyValue(element, out var propertValue))
                    return filter.Operation.OperationExecute(propertValue, filter.Value);
                return false;
            };
        }

        public Func<TElement, bool> Convert(DoublePropertyFilter filter)
        {
            return element =>
            {
                if (filter.TryGetPropertyValue(element, out var propertValue))
                    return filter.Operation.OperationExecute(propertValue, filter.Value);
                return false;
            };
        }

        public Func<TElement, bool> Convert(DecimalPropertyFilter filter)
        {
            return element =>
            {
                if (filter.TryGetPropertyValue(element, out var propertValue))
                    return filter.Operation.OperationExecute(propertValue, filter.Value);
                return false;
            };
        }

        public Func<TElement, bool> Convert(DateTimePropertyFilter filter)
        {
            return element =>
            {
                if (filter.TryGetPropertyValue(element, out var propertValue))
                    return filter.Operation.OperationExecute(propertValue, filter.Value);
                return false;
            };
        }

        public Func<TElement, bool> Convert(BoolPropertyFilter filter)
        {
            return element =>
            {
                if (filter.TryGetPropertyValue(element, out var propertValue))
                    return filter.Operation.OperationExecute(propertValue, filter.Value);
                return false;
            };
        }
    }
}
