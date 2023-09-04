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
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Id)).ColumnName}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Access)).ColumnName}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Cost)).ColumnName}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Name)).ColumnName}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Lumen)).ColumnName}) " +
                $"values ('{deskLamp.Id}', '{Enum.GetName((DbAccessRigts)deskLamp.Access)}', {deskLamp.Cost.ToString(CultureInfo.GetCultureInfo("en-US"))}," +
                $"'{deskLamp.Name}', {deskLamp.Lumen});";
        }

        public string CreateUpdateQuery(Element element, DbTableColumnsMap propertyMap)
        {
            var deskLamp = (DeskLamp)element;
            return $"update {propertyMap.TableName} set " +                
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Access)).ColumnName} = '{Enum.GetName((DbAccessRigts)deskLamp.Access)}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Cost)).ColumnName} = {deskLamp.Cost.ToString(CultureInfo.GetCultureInfo("en-US"))}," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Name)).ColumnName} = '{deskLamp.Name}'," +
                $"{propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Lumen)).ColumnName} = {deskLamp.Lumen}" +
                $" where {propertyMap.GetPropertyNameInDb(nameof(DeskLamp.Id)).ColumnName} = '{deskLamp.Id}';";
        }        
    }
}
