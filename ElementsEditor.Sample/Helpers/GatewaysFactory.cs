using ElementsEditor.Gateway.PostgresDb;
using ElementsEditor.Sample.Configuration;
using ElementsEditor.Sample.Models;
using ElementsEditor.Sample.PostgresDb;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor.Sample.Helpers
{
    internal class GatewaysFactory
    {
        public static IElementsGateway<Product> CreatePostgres(IConfigurationRoot config)
        {
            DbTableColumnsMapConfigurator configurator = new DbTableColumnsMapConfigurator();
            var tableColumnsMap = configurator.Configure(config);
            DbElementMapsBuilder elementMapBuilder = new DbElementMapsBuilder();
            elementMapBuilder.Add(new DbDeskLampMap()).Add(new DbFridgeMap()).Add(new DbKettleMap());
            return new DbGateway<Product>(config.GetConnectionString("Default")!,
                                          tableColumnsMap,
                                          elementMapBuilder.Build());
        }

        public static IElementsGateway<Product> CreateInMemory(IConfigurationRoot config)
        {
            int count = 1000;
            var products = new List<Product>(count);
            for (int i = 0; i < count; i++)
            {                
                if (i % 3 == 0)
                    products.Add(new DeskLamp(Guid.NewGuid().ToString(), AccessRights.All, (Decimal)1.24 * i, $"Electros sa-{i}", i + 259 / 2));
                else if (i % 2 == 0)
                    products.Add(new Fridge(Guid.NewGuid().ToString(), AccessRights.All, (Decimal)1.24 * i, $"Indesit LX-{i}", -(i % 100 + 13)));
                else
                    products.Add(new Kettle(Guid.NewGuid().ToString(), AccessRights.All, (Decimal)1.24 * i, $"Pollaris-{i}", 1800 - i % 100));
            }
            return new ElementsCollectionGateway<Product>(products)
            { DebugDelay = 600 };
        }

    }
}
