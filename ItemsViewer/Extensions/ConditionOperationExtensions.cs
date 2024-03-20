using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor
{
    public static class ConditionOperationExtensions
    {
        public static bool OperationExecute(this ConditionOperation operation, string? propertyValue, string? filterValue)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue == filterValue;
                case ConditionOperation.NotEquals:
                    return propertyValue != filterValue;
                case ConditionOperation.Contains:
                    return propertyValue?.Contains(filterValue) == true;
                case ConditionOperation.StartWith:
                    return propertyValue?.StartsWith(filterValue) == true;
                case ConditionOperation.EndWith:
                    return propertyValue?.EndsWith(filterValue) == true;
                case ConditionOperation.Large:
                case ConditionOperation.LargeOrEquals:
                case ConditionOperation.Less:
                case ConditionOperation.LessOrEquals:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for string type");
            }
        }

        public static bool OperationExecute(this ConditionOperation operation, int propertyValue, int filterValue)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue == filterValue;
                case ConditionOperation.NotEquals:
                    return propertyValue != filterValue;
                case ConditionOperation.Large:
                    return propertyValue > filterValue;
                case ConditionOperation.LargeOrEquals:
                    return propertyValue >= filterValue;
                case ConditionOperation.Less:
                    return propertyValue < filterValue;
                case ConditionOperation.LessOrEquals:
                    return propertyValue <= filterValue;
                case ConditionOperation.Contains:
                case ConditionOperation.StartWith:
                case ConditionOperation.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for int type");
            }
        }

        public static bool OperationExecute(this ConditionOperation operation, decimal propertyValue, decimal filterValue)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue == filterValue;
                case ConditionOperation.NotEquals:
                    return propertyValue != filterValue;
                case ConditionOperation.Large:
                    return propertyValue > filterValue;
                case ConditionOperation.LargeOrEquals:
                    return propertyValue >= filterValue;
                case ConditionOperation.Less:
                    return propertyValue < filterValue;
                case ConditionOperation.LessOrEquals:
                    return propertyValue <= filterValue;
                case ConditionOperation.Contains:
                case ConditionOperation.StartWith:
                case ConditionOperation.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for decimal type");
            }
        }

        public static bool OperationExecute(this ConditionOperation operation, double propertyValue, double filterValue)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue == filterValue;
                case ConditionOperation.NotEquals:
                    return propertyValue != filterValue;
                case ConditionOperation.Large:
                    return propertyValue > filterValue;
                case ConditionOperation.LargeOrEquals:
                    return propertyValue >= filterValue;
                case ConditionOperation.Less:
                    return propertyValue < filterValue;
                case ConditionOperation.LessOrEquals:
                    return propertyValue <= filterValue;
                case ConditionOperation.Contains:
                case ConditionOperation.StartWith:
                case ConditionOperation.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for double type");
            }
        }

        public static bool OperationExecute(this ConditionOperation operation, DateTime propertyValue, DateTime filterValue)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue == filterValue;
                case ConditionOperation.NotEquals:
                    return propertyValue != filterValue;
                case ConditionOperation.Large:
                    return propertyValue > filterValue;
                case ConditionOperation.LargeOrEquals:
                    return propertyValue >= filterValue;
                case ConditionOperation.Less:
                    return propertyValue < filterValue;
                case ConditionOperation.LessOrEquals:
                    return propertyValue <= filterValue;
                case ConditionOperation.Contains:
                case ConditionOperation.StartWith:
                case ConditionOperation.EndWith:
                default:
                    throw new ArgumentException($"Invalidate ConditionOperation - '{operation}', for DateTime type");
            }
        }

        public static bool OperationExecute(this ConditionOperation operation, bool propertyValue, bool filterValue)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return propertyValue == filterValue;
                case ConditionOperation.NotEquals:
                    return propertyValue != filterValue;
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
