using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ElementsEditor.Gateway.PostgresDb
{
    public sealed class DbTableColumnsMap
    {
        private readonly List<DbColumnMap> _map;
        public string TableName { get; }
        public string DeterminatorColumnName { get; }
        public int DeterminatorColumnIndex { get; }
        public IReadOnlyDictionary<Type, string> DeterminatorMap { get; }

        public DbTableColumnsMap(
            IEnumerable<DbColumnMap> map,
            IReadOnlyDictionary<Type, string> determinatorMap,
            string tableName,            
            string determinatorColumnName,
            int determinatorColumnIndex)
        {
            _map = new List<DbColumnMap>(map);
            TableName = tableName;
            DeterminatorMap = determinatorMap;
            DeterminatorColumnName = determinatorColumnName;
            DeterminatorColumnIndex = determinatorColumnIndex;
        }

        public DbColumnMap GetPropertyNameInDb(string propertyName)
        {            
            return _map.FirstOrDefault(a => a.PropertyName == propertyName) 
                ?? throw new ArgumentException("Unknown property name");
        }

        public DbColumnMap GetPropertyNameInModel(string propertyName)
        {
            return _map.FirstOrDefault(a => a.ColumnName == propertyName)
                ?? throw new ArgumentException("Unknown property name");
        }
    }
}
