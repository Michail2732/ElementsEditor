using System;
using System.Collections.Generic;
using System.Text;

namespace ElementsEditor.Gateway.PostgresDb
{
    public sealed class DbElementMapsBuilder
    {
        private readonly List<IDbElementMap> _maps = new List<IDbElementMap>();

        public DbElementMapsBuilder Add(IDbElementMap map)
        {
            if (_maps.Contains(map))
                throw new ArgumentException("Map already exists");
            _maps.Add(map);
            return this;
        }

        public DbElementMaps Build()
        {
            return new DbElementMaps(_maps);
        }
    }
}
