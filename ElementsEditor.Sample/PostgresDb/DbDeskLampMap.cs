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
    public class DbDeskLampMap : IDbElementMap
    {
        public bool CanCreateQuery(Element element) => element is DeskLamp;        

        public bool CanMapToModel(NpgsqlDataReader reader, DbTableColumnsMap propertyMap)
        {
            return reader.GetFieldValue<string>(propertyMap.DeterminatorColumnIndex) == propertyMap.DeterminatorMap[typeof(DeskLamp)];
        }

        public Element MapToModel(NpgsqlDataReader reader, DbTableColumnsMap propertyMap)
        {
            return new DeskLamp(
                reader.GetFieldValue<Guid>(propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Id)).ColumnIndex).ToString(),
                (AccessRights)Enum.Parse<DbAccessRigts>(reader.GetFieldValue<string>(propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Access)).ColumnIndex)),
                reader.GetFieldValue<decimal>(propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Cost)).ColumnIndex),
                reader.GetFieldValue<string>(propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Name)).ColumnIndex),
                reader.GetFieldValue<int>(propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Lumen)).ColumnIndex)
                );
        }

        public string CreateInsertQuery(Element element, DbTableColumnsMap propertyMap)
        {
            var deskLamp = (DeskLamp)element;
            return $"insert into {propertyMap.TableName} (" +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Id))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Access))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Cost))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Name))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Lumen))}) " +
                $"values ('{deskLamp.Id}', '{Enum.GetName(deskLamp.Access)}', {deskLamp.Cost}," +
                $"'{deskLamp.Name}', {deskLamp.Lumen})";
        }

        public string CreateUpdateQuery(Element element, DbTableColumnsMap propertyMap)
        {
            var deskLamp = (DeskLamp)element;
            return $"update {propertyMap.TableName} set " +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Id))} = '{deskLamp.Id}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Access))} = '{Enum.GetName(deskLamp.Access)}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Cost))} = {deskLamp.Cost}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Name))} = '{deskLamp.Name}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Lumen))} = {deskLamp.Lumen}";
        }        
    }
}
