using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor
{
    internal class FuncFilterExecutor<TElement>: IPropertyFilterExecutor<TElement, bool>
        where TElement : Element
    {
        private readonly IReadOnlyList<IPropertyFilter> _filters;

        public FuncFilterExecutor(IReadOnlyList<IPropertyFilter> filters)
        {
            _filters = filters ?? throw new ArgumentNullException(nameof(filters));
        }

        public bool Execute(TElement element)
        {
            if (_filters.Count == 0)
                return true;
            bool isSuccess = _filters[0].Execute(element, this);
            for (int i = 1; i < _filters.Count; i++)
            {
                bool localRes = _filters[i].Execute(element, this);
                if (_filters[i].Logic == Logic.Or && isSuccess)
                    return true;
                else if (_filters[i].Logic == Logic.Or)
                    isSuccess = localRes;
                else
                    isSuccess &= localRes;
            }
            return isSuccess;
        }

        public bool Execute(TElement element, StringPropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }

        public bool Execute(TElement element, IntPropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }

        public bool Execute(TElement element, DoublePropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }

        public bool Execute(TElement element, DecimalPropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }

        public bool Execute(TElement element, DateTimePropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }

        public bool Execute(TElement element, BoolPropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }        

        private bool OperationExecute(string? propertyValue1, string? propertyValue2, ConditionOperation operation)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperation.NotEquals:
                    return propertyValue1 != propertyValue2;
                case ConditionOperation.Contains:
                    return propertyValue1?.Contains(propertyValue2) == true;
                case ConditionOperation.StartWith:
                    return propertyValue1?.StartsWith(propertyValue2) == true;
                case ConditionOperation.EndWith:
                    return propertyValue1?.EndsWith(propertyValue2) == true;
                case ConditionOperation.Large:
                case ConditionOperation.LargeOrEquals:
                case ConditionOperation.Less:
                case ConditionOperation.LessOrEquals:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for string type");
            }
        }

        private bool OperationExecute(int propertyValue1, int propertyValue2, ConditionOperation operation)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperation.NotEquals:
                    return propertyValue1 != propertyValue2;                
                case ConditionOperation.Large:
                    return propertyValue1 > propertyValue2;
                case ConditionOperation.LargeOrEquals:
                    return propertyValue1 >= propertyValue2;
                case ConditionOperation.Less:
                    return propertyValue1 < propertyValue2;
                case ConditionOperation.LessOrEquals:
                    return propertyValue1 <= propertyValue2;
                case ConditionOperation.Contains:
                case ConditionOperation.StartWith:                    
                case ConditionOperation.EndWith:                    
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for int type");
            }
        }

        private bool OperationExecute(decimal propertyValue1, decimal propertyValue2, ConditionOperation operation)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperation.NotEquals:
                    return propertyValue1 != propertyValue2;
                case ConditionOperation.Large:
                    return propertyValue1 > propertyValue2;
                case ConditionOperation.LargeOrEquals:
                    return propertyValue1 >= propertyValue2;
                case ConditionOperation.Less:
                    return propertyValue1 < propertyValue2;
                case ConditionOperation.LessOrEquals:
                    return propertyValue1 <= propertyValue2;
                case ConditionOperation.Contains:
                case ConditionOperation.StartWith:
                case ConditionOperation.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for decimal type");
            }
        }

        private bool OperationExecute(double propertyValue1, double propertyValue2, ConditionOperation operation)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperation.NotEquals:
                    return propertyValue1 != propertyValue2;
                case ConditionOperation.Large:
                    return propertyValue1 > propertyValue2;
                case ConditionOperation.LargeOrEquals:
                    return propertyValue1 >= propertyValue2;
                case ConditionOperation.Less:
                    return propertyValue1 < propertyValue2;
                case ConditionOperation.LessOrEquals:
                    return propertyValue1 <= propertyValue2;
                case ConditionOperation.Contains:
                case ConditionOperation.StartWith:
                case ConditionOperation.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for double type");
            }
        }

        private bool OperationExecute(DateTime propertyValue1, DateTime propertyValue2, ConditionOperation operation)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperation.NotEquals:
                    return propertyValue1 != propertyValue2;
                case ConditionOperation.Large:
                    return propertyValue1 > propertyValue2;
                case ConditionOperation.LargeOrEquals:
                    return propertyValue1 >= propertyValue2;
                case ConditionOperation.Less:
                    return propertyValue1 < propertyValue2;
                case ConditionOperation.LessOrEquals:
                    return propertyValue1 <= propertyValue2;
                case ConditionOperation.Contains:
                case ConditionOperation.StartWith:
                case ConditionOperation.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for DateTime type");
            }
        }

        private bool OperationExecute(bool propertyValue1, bool propertyValue2, ConditionOperation operation)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperation.NotEquals:
                    return propertyValue1 != propertyValue2;
                case ConditionOperation.Large:                    
                case ConditionOperation.LargeOrEquals:                    
                case ConditionOperation.Less:                    
                case ConditionOperation.LessOrEquals:                    
                case ConditionOperation.Contains:
                case ConditionOperation.StartWith:
                case ConditionOperation.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for bool type");
            }
        }        
    }
}
