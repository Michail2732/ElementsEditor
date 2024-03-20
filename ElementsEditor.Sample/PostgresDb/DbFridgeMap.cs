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
    public class DbFridgeMap: IDbElementMap
    {
        public bool CanCreateQuery(Element element) => element is Fridge;

        public bool CanMapToModel(NpgsqlDataReader reader, DbTableColumnsMap propertyMap)
        {
            return reader.GetFieldValue<string>(propertyMap.DeterminatorColumnIndex) == propertyMap.DeterminatorMap[typeof(Fridge)];
        }

        public Element MapToModel(NpgsqlDataReader reader, DbTableColumnsMap propertyMap)
        {
            return new Fridge(
                reader.GetFieldValue<Guid>(propertyMap.GetPropertyNameInDb(nameof(Fridge.Id)).ColumnIndex).ToString(),                
                reader.GetFieldValue<decimal>(propertyMap.GetPropertyNameInDb(nameof(Fridge.Cost)).ColumnIndex),
                reader.GetFieldValue<string>(propertyMap.GetPropertyNameInDb(nameof(Fridge.Name)).ColumnIndex),
                reader.GetFieldValue<int>(propertyMap.GetPropertyNameInDb(nameof(Fridge.Temperature)).ColumnIndex)
                );
        }

        public string CreateInsertQuery(Element element, DbTableColumnsMap propertyMap)
        {
            var fridge = (Fridge)element;
            return $"insert into {propertyMap.TableName} (" +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Id)).ColumnName}," +               
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Cost)).ColumnName}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Name)).ColumnName}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Temperature)).ColumnName}) " +
                $"values ('{fridge.Id}', {fridge.Cost.ToString(CultureInfo.GetCultureInfo("en-US"))}," +
                $"'{fridge.Name}', {fridge.Temperature});";
        }

        public string CreateUpdateQuery(Element element, DbTableColumnsMap propertyMap)
        {
            var fridge = (Fridge)element;
            return $"update {propertyMap.TableName} set " +                                
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Cost)).ColumnName} = {fridge.Cost.ToString(CultureInfo.GetCultureInfo("en-US"))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Name)).ColumnName} = '{fridge.Name}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Temperature)).ColumnName} = {fridge.Temperature}" +
                $" where {propertyMap.GetPropertyNameInDb(nameof(Fridge.Id)).ColumnName} = '{fridge.Id}';";
        }
    }
}
