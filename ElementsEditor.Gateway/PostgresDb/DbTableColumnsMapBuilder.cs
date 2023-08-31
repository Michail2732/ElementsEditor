using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor.Gateway.PostgresDb
{
    public sealed class DbTableColumnsMapBuilder
    {
        private readonly List<DbColumnMap> _map = new List<DbColumnMap>();
        private readonly Dictionary<Type, string> _determinatorMap = new Dictionary<Type, string>();
        private string _tableName;
        private string _determinatorColumnName;
        private int _determinatorColumnIndex;

        public DbTableColumnsMapBuilder SetTableName(string tableName)
        {
            _tableName = tableName;
            return this;
        }
        public DbTableColumnsMapBuilder SetDeterminatorName(string determinatorName)
        {
            _determinatorColumnName = determinatorName;
            return this;
        }
        public DbTableColumnsMapBuilder SetDeterminatorIndex(int index)
        {
            _determinatorColumnIndex = index;
            return this;
        }

        public DbTableColumnsMapBuilder AddDeterminatorMap(Type type, string determinatorValue)
        {
            _determinatorMap[type] = determinatorValue;
            return this;
        }

        public DbTableColumnsMapBuilder AddColumnMap(string columnName, string propertyName, int columnIndex)
        {
            _map.Add(new DbColumnMap(columnName, propertyName, columnIndex));
            return this;
        }

        public DbTableColumnsMap Build()
        {
            return new DbTableColumnsMap(_map, _determinatorMap, _tableName, _determinatorColumnName, _determinatorColumnIndex);
        }

    }
}
