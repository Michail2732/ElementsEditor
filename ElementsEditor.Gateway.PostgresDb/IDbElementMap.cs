using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor.Gateway.PostgresDb
{
    public interface IDbElementMap
    {
        bool CanMapToModel(NpgsqlDataReader reader, DbTableColumnsMap propertyMap);
        Element MapToModel(NpgsqlDataReader reader, DbTableColumnsMap propertyMap);
        bool CanCreateQuery(Element element);
        string CreateUpdateQuery(Element element, DbTableColumnsMap propertyMap);
        string CreateInsertQuery(Element element, DbTableColumnsMap propertyMap);
    }
}
