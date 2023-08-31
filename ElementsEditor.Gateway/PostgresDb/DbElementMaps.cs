using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElementsEditor.Gateway.PostgresDb
{
    public class DbElementMaps
    {
        private readonly IDbElementMap[] _maps;

        public DbElementMaps(IEnumerable<IDbElementMap> maps)
        {
            _maps = maps.ToArray();
        }

        public IDbElementMap GetMap(NpgsqlDataReader reader, DbTableColumnsMap columnsMap)
        {
            return _maps.FirstOrDefault(a => a.CanMapToModel(reader, columnsMap))
                ?? throw new ArgumentException("Coulnt found match map");
        }

        public IDbElementMap GetMap(Element product)
        {
            return _maps.FirstOrDefault(a => a.CanCreateQuery(product))
                ?? throw new ArgumentException("Coulnt found match map");
        }
    }
}
