using System;

namespace ElementsEditor.Gateway.PostgresDb
{
    public class DbColumnMap
    {
        public readonly string ColumnName;
        public readonly string PropertyName;
        public readonly int ColumnIndex;

        public DbColumnMap(string columnName, string propertyName, int columnIndex)
        {
            ColumnName = columnName ?? throw new ArgumentNullException(nameof(columnName));
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            ColumnIndex = columnIndex;
        }
    }
}
