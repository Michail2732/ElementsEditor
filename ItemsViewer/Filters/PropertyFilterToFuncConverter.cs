using System;
using System.Collections.Generic;
using System.Text;

namespace ItemsViewer.Filters
{
    public class PropertyFilterToFuncConverter<TItem> : IPropertyFilterConverter<Func<TItem, bool>>
        where TItem : Item
    {
        public Func<TItem, bool> Convert(IReadOnlyList<IPropertyFilter> filters)
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

        public Func<TItem, bool> Convert(StringPropertyFilter filter)
        {
            return element =>
            {
                if (filter.TryGetPropertyValue(element, out var propertValue))
                    return filter.Operation.OperationExecute(propertValue, filter.Value);
                return false;
            };
        }

        public Func<TItem, bool> Convert(IntPropertyFilter filter)
        {
            return element =>
            {
                if (filter.TryGetPropertyValue(element, out var propertValue))
                    return filter.Operation.OperationExecute(propertValue, filter.Value);
                return false;
            };
        }

        public Func<TItem, bool> Convert(DoublePropertyFilter filter)
        {
            return element =>
            {
                if (filter.TryGetPropertyValue(element, out var propertValue))
                    return filter.Operation.OperationExecute(propertValue, filter.Value);
                return false;
            };
        }

        public Func<TItem, bool> Convert(DecimalPropertyFilter filter)
        {
            return element =>
            {
                if (filter.TryGetPropertyValue(element, out var propertValue))
                    return filter.Operation.OperationExecute(propertValue, filter.Value);
                return false;
            };
        }

        public Func<TItem, bool> Convert(DateTimePropertyFilter filter)
        {
            return element =>
            {
                if (filter.TryGetPropertyValue(element, out var propertValue))
                    return filter.Operation.OperationExecute(propertValue, filter.Value);
                return false;
            };
        }

        public Func<TItem, bool> Convert(BoolPropertyFilter filter)
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
