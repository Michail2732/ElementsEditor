using ElementsEditor.Gateway.PostgresDb;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor.Sample.Configuration
{
    public class DbTableColumnsMapConfigurator
    {
        public DbTableColumnsMap Configure(IConfigurationRoot configuration)
        {            
            DbTableColumnsMapBuilder builder = new DbTableColumnsMapBuilder();
            var tableMapConfigSection = configuration.GetSection("TableMapConfiguration");
            builder.SetTableName(tableMapConfigSection["TableName"]!);
            builder.SetDeterminatorName(tableMapConfigSection["DeterminatorColumnName"]!);
            builder.SetDeterminatorIndex(int.Parse(tableMapConfigSection["DeterminatorColumnIndex"]!));

            var determenatorsSection = tableMapConfigSection.GetSection("Determinators");
            foreach (var determinatorConfig in determenatorsSection.GetChildren())            
                builder.AddDeterminatorMap(Type.GetType(determinatorConfig.Key)!, determinatorConfig.Value!);

            var columnsMapSection = tableMapConfigSection.GetSection("Columns");
            foreach (var columnMapConfig in columnsMapSection.GetChildren())
            {
                builder.AddColumnMap(
                    columnMapConfig["ColumnName"]!,
                    columnMapConfig["PropertyName"]!,
                    int.Parse(columnMapConfig["ColumnIndex"]!));
            }
            return builder.Build();
        }
    }
}
