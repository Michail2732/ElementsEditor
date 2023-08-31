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
    internal class DbRebarMap : IDbElementMap
    {
        public bool CanCreateQuery(Element element) => element is Rebar;

        public bool CanMapToModel(NpgsqlDataReader reader, DbTableColumnsMap propertyMap)
        {
            return reader.GetFieldValue<string>(propertyMap.DeterminatorColumnIndex) == propertyMap.DeterminatorMap[typeof(Rebar)];
        }

        public Element MapToModel(NpgsqlDataReader reader, DbTableColumnsMap propertyMap)
        {
            return new Rebar(
                reader.GetFieldValue<Guid>(propertyMap.GetPropertyNameInDb(nameof(Rebar.Id)).ColumnIndex).ToString(),
                (AccessRights)Enum.Parse<DbAccessRigts>(reader.GetFieldValue<string>(propertyMap.GetPropertyNameInDb(nameof(Rebar.Access)).ColumnIndex)),
                reader.GetFieldValue<decimal>(propertyMap.GetPropertyNameInDb(nameof(Rebar.Cost)).ColumnIndex),
                reader.GetFieldValue<string>(propertyMap.GetPropertyNameInDb(nameof(Rebar.Name)).ColumnIndex),
                reader.GetFieldValue<string>(propertyMap.GetPropertyNameInDb(nameof(Rebar.Type)).ColumnIndex)
                );
        }

        public string CreateInsertQuery(Element element, DbTableColumnsMap propertyMap)
        {
            var rebar = (Rebar)element;
            return $"insert into {propertyMap.TableName} (" +
                $"{propertyMap.GetPropertyNameInDb(nameof(Rebar.Id))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Rebar.Access))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Rebar.Cost))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Rebar.Name))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Rebar.Type))}) " +
                $"values ('{rebar.Id}', '{Enum.GetName(rebar.Access)}', {rebar.Cost}," +
                $"'{rebar.Name}', '{rebar.Type}')";
        }

        public string CreateUpdateQuery(Element element, DbTableColumnsMap propertyMap)
        {
            var rebar = (Rebar)element;
            return $"update {propertyMap.TableName} set " +
                $"{propertyMap.GetPropertyNameInDb(nameof(Rebar.Id))} = '{rebar.Id}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Rebar.Access))} = '{Enum.GetName(rebar.Access)}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Rebar.Cost))} = {rebar.Cost}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Rebar.Name))} = '{rebar.Name}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(Rebar.Type))} = '{rebar.Type}'";
        }
    }
}
