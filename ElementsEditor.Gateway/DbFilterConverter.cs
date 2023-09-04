using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor.Gateway.PostgresDb
{
    internal class DbFilterConverter : IPropertyFilterConverter<string>
    {
        private readonly DbTableColumnsMap _map;
        private bool _isFirstCondition;

        internal DbTableColumnsMap PropertyMap => _map;

        public DbFilterConverter(DbTableColumnsMap map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        public string Convert(IReadOnlyList<IPropertyFilter> filters)
        {
            StringBuilder result = new StringBuilder();
            _isFirstCondition = true;
            for (int i = 0; i < filters.Count; i++)
            {
                result.Append(filters[i].Convert(this));
                _isFirstCondition = false;
            }
            return result.ToString();
        }

        public string Convert(StringPropertyFilter filter)
        {
            return $" {LogicToDb(filter.Logic)} {_map.GetPropertyNameInDb(filter.PropertyName).ColumnName} {OperatorToDb(filter.Operation, filter.Value ?? "")} ";
        }

        public string Convert(IntPropertyFilter filter)
        {
            return $" {LogicToDb(filter.Logic)} {_map.GetPropertyNameInDb(filter.PropertyName).ColumnName} {OperatorToDb(filter.Operation)} {filter.Value}";
        }

        public string Convert(DoublePropertyFilter filter)
        {
            return $" {LogicToDb(filter.Logic)} {_map.GetPropertyNameInDb(filter.PropertyName).ColumnName} {OperatorToDb(filter.Operation)} {filter.Value}";
        }

        public string Convert(DecimalPropertyFilter filter)
        {
            return $" {LogicToDb(filter.Logic)} {_map.GetPropertyNameInDb(filter.PropertyName).ColumnName} {OperatorToDb(filter.Operation)} {filter.Value}";
        }

        public string Convert(DateTimePropertyFilter filter)
        {
            return $" {LogicToDb(filter.Logic)} {_map.GetPropertyNameInDb(filter.PropertyName).ColumnName} {OperatorToDb(filter.Operation)} {filter.Value}";
        }

        public string Convert(BoolPropertyFilter filter)
        {
            return $" {LogicToDb(filter.Logic)} {_map.GetPropertyNameInDb(filter.PropertyName).ColumnName} {OperatorToDb(filter.Operation)} {filter.Value}";
        }

        private string LogicToDb(Logic logic)
        {
            if (_isFirstCondition)
                return string.Empty;
            return logic == Logic.And ? "and" : "or";
        }

        private string OperatorToDb(ConditionOperation operation, string? value = null)
        {
            switch (operation)
            {
                case ConditionOperation.Equals:
                    return value != null ? $"like '{value}'" : "=";
                case ConditionOperation.NotEquals:
                    return value != null ? $"not like '{value}'" : "<>";
                case ConditionOperation.Large:
                    return ">";
                case ConditionOperation.LargeOrEquals:
                    return ">=";
                case ConditionOperation.Less:
                    return "<";
                case ConditionOperation.LessOrEquals:
                    return "<=";
                case ConditionOperation.Contains:
                    return $"like '%{value}%'";
                case ConditionOperation.StartWith:
                    return $"like '{value}%'";
                case ConditionOperation.EndWith:
                    return $"like '%{value}'";
                default:
                    throw new ArgumentException("Unknown operation condition");
            }
        }
    }
}
