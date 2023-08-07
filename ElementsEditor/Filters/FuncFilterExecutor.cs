using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor
{
    internal class FuncFilterExecutor<TElement>
        where TElement: Element
    {
        private readonly IReadOnlyList<IFilter> _filters;

        public FuncFilterExecutor(IReadOnlyList<IFilter> filters)
        {
            _filters = filters ?? throw new ArgumentNullException(nameof(filters));
        }

        public bool Execute(TElement element)
        {
            if (_filters.Count == 0)
                return true;
            bool isSuccess = ExecutePrivate(element, _filters[0]);
            for (int i = 1; i < _filters.Count; i++)
            {
                bool localRes = ExecutePrivate(element, _filters[i]);
                if (_filters[i].Logic == Logic.Or && isSuccess)
                    return true;
                else if (_filters[i].Logic == Logic.Or)
                    isSuccess = localRes;
                else
                    isSuccess &= localRes;
            }
            return isSuccess;
        }

        private bool ExecutePrivate(TElement element, IFilter filter)
        {
            switch (filter.ValueType)
            {
                case ValueType.String:
                    if (ElementPropertySolver<TElement, string>.TrySolveProperty(element, filter.PropertyName, out var strPropertyValue))
                    {
                        var filterValue = (string?)filter.Value;
                        if (filterValue != null)
                            return OperationExecute(strPropertyValue, filterValue, filter.Operation);
                    }                        
                    break;
                case ValueType.Integer:
                    if (ElementPropertySolver<TElement, int>.TrySolveProperty(element, filter.PropertyName, out var intPropertyValue))
                    {
                        var filterValue = (int?)filter.Value;
                        if (filterValue != null)
                            return OperationExecute(intPropertyValue, filterValue.Value, filter.Operation);
                    }                        
                    break;
                case ValueType.Double:
                    if (ElementPropertySolver<TElement, double>.TrySolveProperty(element, filter.PropertyName, out var dblPropertyValue))
                    {
                        var filterValue = (double?)filter.Value;
                        if (filterValue != null)
                            return OperationExecute(dblPropertyValue, filterValue.Value, filter.Operation);
                    }                        
                    break;
                case ValueType.Decimal:
                    if (ElementPropertySolver<TElement, decimal>.TrySolveProperty(element, filter.PropertyName, out var dcmlPropertyValue))
                    {
                        var filterValue = (decimal?)filter.Value;
                        if (filterValue != null)
                            return OperationExecute(dcmlPropertyValue, filterValue.Value, filter.Operation);
                    }                        
                    break;
                case ValueType.Boolean:
                    if (ElementPropertySolver<TElement, bool>.TrySolveProperty(element, filter.PropertyName, out var boolPropertyValue))
                    {
                        var filterValue = (bool?)filter.Value;
                        if (filterValue != null)
                            return OperationExecute(boolPropertyValue, filterValue.Value, filter.Operation);
                    }                    
                    break;
                case ValueType.DateTime:
                    if (ElementPropertySolver<TElement, DateTime>.TrySolveProperty(element, filter.PropertyName, out var dtPropertyValue))
                    {
                        var filterValue = (DateTime?)filter.Value;
                        if (filterValue != null)
                            return OperationExecute(dtPropertyValue, filterValue.Value, filter.Operation);
                    }                        
                    break;
                default:
                    break;
            }
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

    }
}
