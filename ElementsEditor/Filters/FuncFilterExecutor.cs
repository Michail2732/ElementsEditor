using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor
{
    internal class FuncFilterExecutor: IPropertyFilterExecutor<bool>        
    {
        private readonly IReadOnlyList<IPropertyFilter> _filters;

        public FuncFilterExecutor(IReadOnlyList<IPropertyFilter> filters)
        {
            _filters = filters ?? throw new ArgumentNullException(nameof(filters));
        }

        public bool Execute(Element element)
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

        public bool Execute(Element element, StringPropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }

        public bool Execute(Element element, IntPropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }

        public bool Execute(Element element, DoublePropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }

        public bool Execute(Element element, DecimalPropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }

        public bool Execute(Element element, DateTimePropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }

        public bool Execute(Element element, BoolPropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }

        public bool Execute(Element element, CustomPropertyFilter filter)
        {
            if (filter.TryGetPropertyValue(element, out var propertValue))
                return OperationExecute(propertValue, filter.Value, filter.Operation);
            return false;
        }      

        private bool OperationExecute(string? propertyValue1, string? propertyValue2, ConditionOperatioin operation)
        {
            switch (operation)
            {
                case ConditionOperatioin.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperatioin.NotEquals:
                    return propertyValue1 != propertyValue2;
                case ConditionOperatioin.Contains:
                    return propertyValue1?.Contains(propertyValue2) == true;
                case ConditionOperatioin.StartWith:
                    return propertyValue1?.StartsWith(propertyValue2) == true;
                case ConditionOperatioin.EndWith:
                    return propertyValue1?.EndsWith(propertyValue2) == true;
                case ConditionOperatioin.Large:
                case ConditionOperatioin.LargeOrEquals:
                case ConditionOperatioin.Less:
                case ConditionOperatioin.LessOrEquals:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for string type");
            }
        }

        private bool OperationExecute(int propertyValue1, int propertyValue2, ConditionOperatioin operation)
        {
            switch (operation)
            {
                case ConditionOperatioin.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperatioin.NotEquals:
                    return propertyValue1 != propertyValue2;                
                case ConditionOperatioin.Large:
                    return propertyValue1 > propertyValue2;
                case ConditionOperatioin.LargeOrEquals:
                    return propertyValue1 >= propertyValue2;
                case ConditionOperatioin.Less:
                    return propertyValue1 < propertyValue2;
                case ConditionOperatioin.LessOrEquals:
                    return propertyValue1 <= propertyValue2;
                case ConditionOperatioin.Contains:
                case ConditionOperatioin.StartWith:                    
                case ConditionOperatioin.EndWith:                    
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for int type");
            }
        }

        private bool OperationExecute(decimal propertyValue1, decimal propertyValue2, ConditionOperatioin operation)
        {
            switch (operation)
            {
                case ConditionOperatioin.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperatioin.NotEquals:
                    return propertyValue1 != propertyValue2;
                case ConditionOperatioin.Large:
                    return propertyValue1 > propertyValue2;
                case ConditionOperatioin.LargeOrEquals:
                    return propertyValue1 >= propertyValue2;
                case ConditionOperatioin.Less:
                    return propertyValue1 < propertyValue2;
                case ConditionOperatioin.LessOrEquals:
                    return propertyValue1 <= propertyValue2;
                case ConditionOperatioin.Contains:
                case ConditionOperatioin.StartWith:
                case ConditionOperatioin.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for decimal type");
            }
        }

        private bool OperationExecute(double propertyValue1, double propertyValue2, ConditionOperatioin operation)
        {
            switch (operation)
            {
                case ConditionOperatioin.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperatioin.NotEquals:
                    return propertyValue1 != propertyValue2;
                case ConditionOperatioin.Large:
                    return propertyValue1 > propertyValue2;
                case ConditionOperatioin.LargeOrEquals:
                    return propertyValue1 >= propertyValue2;
                case ConditionOperatioin.Less:
                    return propertyValue1 < propertyValue2;
                case ConditionOperatioin.LessOrEquals:
                    return propertyValue1 <= propertyValue2;
                case ConditionOperatioin.Contains:
                case ConditionOperatioin.StartWith:
                case ConditionOperatioin.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for double type");
            }
        }

        private bool OperationExecute(DateTime propertyValue1, DateTime propertyValue2, ConditionOperatioin operation)
        {
            switch (operation)
            {
                case ConditionOperatioin.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperatioin.NotEquals:
                    return propertyValue1 != propertyValue2;
                case ConditionOperatioin.Large:
                    return propertyValue1 > propertyValue2;
                case ConditionOperatioin.LargeOrEquals:
                    return propertyValue1 >= propertyValue2;
                case ConditionOperatioin.Less:
                    return propertyValue1 < propertyValue2;
                case ConditionOperatioin.LessOrEquals:
                    return propertyValue1 <= propertyValue2;
                case ConditionOperatioin.Contains:
                case ConditionOperatioin.StartWith:
                case ConditionOperatioin.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for DateTime type");
            }
        }

        private bool OperationExecute(bool propertyValue1, bool propertyValue2, ConditionOperatioin operation)
        {
            switch (operation)
            {
                case ConditionOperatioin.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperatioin.NotEquals:
                    return propertyValue1 != propertyValue2;
                case ConditionOperatioin.Large:                    
                case ConditionOperatioin.LargeOrEquals:                    
                case ConditionOperatioin.Less:                    
                case ConditionOperatioin.LessOrEquals:                    
                case ConditionOperatioin.Contains:
                case ConditionOperatioin.StartWith:
                case ConditionOperatioin.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for bool type");
            }
        }

        private bool OperationExecute(object? propertyValue1, object? propertyValue2, ConditionOperatioin operation)
        {
            switch (operation)
            {
                case ConditionOperatioin.Equals:
                    return propertyValue1 == propertyValue2;
                case ConditionOperatioin.NotEquals:
                    return propertyValue1 != propertyValue2;
                case ConditionOperatioin.Large:
                case ConditionOperatioin.LargeOrEquals:
                case ConditionOperatioin.Less:
                case ConditionOperatioin.LessOrEquals:
                case ConditionOperatioin.Contains:
                case ConditionOperatioin.StartWith:
                case ConditionOperatioin.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for custom type");
            }
        }

    }
}
