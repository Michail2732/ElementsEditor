using ElementsEditor.Gateway.PostgresDb;
using ElementsEditor.Sample.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor.Sample.PostgresDb
{
    internal class DbKettleMap : IDbElementMap
    {
        public bool CanCreateQuery(Element element) => element is Kettle;

        public bool CanMapToModel(NpgsqlDataReader reader, DbTableColumnsMap propertyMap)
        {
            return reader.GetFieldValue<string>(propertyMap.DeterminatorColumnIndex) == propertyMap.DeterminatorMap[typeof(Kettle)];
        }

        public Element MapToModel(NpgsqlDataReader reader, DbTableColumnsMap propertyMap)
        {
            return new Kettle(
                reader.GetFieldValue<Guid>(propertyMap.GetPropertyNameInDb(nameof(Kettle.Id)).ColumnIndex).ToString(),                
                reader.GetFieldValue<decimal>(propertyMap.GetPropertyNameInDb(nameof(Kettle.Cost)).ColumnIndex),
                reader.GetFieldValue<string>(propertyMap.GetPropertyNameInDb(nameof(Kettle.Name)).ColumnIndex),
                reader.GetFieldValue<int>(propertyMap.GetPropertyNameInDb(nameof(Kettle.Power)).ColumnIndex)
                );
        }

        public string CreateInsertQuery(Element element, DbTableColumnsMap propertyMap)
        {
            var kettle = (Kettle)element;
            return $"insert into {propertyMap.TableName} (" +
                $"{propertyMap.GetPropertyNameInDb(nameof(Kettle.Id)).ColumnName}," +                
                $"{propertyMap.GetPropertyNameInDb(nameof(Kettle.Cost)).ColumnName}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Kettle.Name)).ColumnName}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Kettle.Power)).ColumnName}) " +
                $"values ('{kettle.Id}', {kettle.Cost.ToString(CultureInfo.GetCultureInfo("en-US"))}," +
                $"'{kettle.Name}', '{kettle.Power}');";
        }

        public string CreateUpdateQuery(Element element, DbTableColumnsMap propertyMap)
        {
            var kettle = (Kettle)element;
            return $"update {propertyMap.TableName} set " +                                
                $"{propertyMap.GetPropertyNameInDb(nameof(Kettle.Cost)).ColumnName} = {kettle.Cost.ToString(CultureInfo.GetCultureInfo("en-US"))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Kettle.Name)).ColumnName} = '{kettle.Name}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Kettle.Power)).ColumnName} = '{kettle.Power}'" +
                $" where {propertyMap.GetPropertyNameInDb(nameof(Kettle.Id)).ColumnName} = '{kettle.Id}';";
        }
    }
}
