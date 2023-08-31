using ElementsEditor.Gateway.PostgresDb;
using ElementsEditor.Sample.Models;
using Npgsql;
using System;
using System.Collections.Generic;
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
                (AccessRights)Enum.Parse<DbAccessRigts>(reader.GetFieldValue<string>(propertyMap.GetPropertyNameInDb(nameof(Fridge.Access)).ColumnIndex)),
                reader.GetFieldValue<decimal>(propertyMap.GetPropertyNameInDb(nameof(Fridge.Cost)).ColumnIndex),
                reader.GetFieldValue<string>(propertyMap.GetPropertyNameInDb(nameof(Fridge.Name)).ColumnIndex),
                reader.GetFieldValue<int>(propertyMap.GetPropertyNameInDb(nameof(Fridge.Temperature)).ColumnIndex)
                );
        }

        public string CreateInsertQuery(Element element, DbTableColumnsMap propertyMap)
        {
            var fridge = (Fridge)element;
            return $"insert into {propertyMap.TableName} (" +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Id))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Access))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Cost))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Name))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Temperature))}) " +
                $"values ('{fridge.Id}', '{Enum.GetName(fridge.Access)}', {fridge.Cost}," +
                $"'{fridge.Name}', {fridge.Temperature})";
        }

        public string CreateUpdateQuery(Element element, DbTableColumnsMap propertyMap)
        {
            var fridge = (Fridge)element;
            return $"update {propertyMap.TableName} set " +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Id))} = '{fridge.Id}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Access))} = '{Enum.GetName(fridge.Access)}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Cost))} = {fridge.Cost}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Name))} = '{fridge.Name}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Fridge.Temperature))} = {fridge.Temperature}";
        }
    }
}
