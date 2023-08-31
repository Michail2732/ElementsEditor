using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ElementsEditor.Gateway.PostgresDb;
using ElementsEditor.Sample.Configuration;
using ElementsEditor.Sample.PostgresDb;
using ElementsEditor.Sample.ViewModels;
using Microsoft.Extensions.Configuration;

namespace ElementsEditor.Sample
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }        

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();                
                DbTableColumnsMapConfigurator configurator = new DbTableColumnsMapConfigurator();
                var tableColumnsMap = configurator.Configure(config);
                DbElementMapsBuilder elementMapBuilder = new DbElementMapsBuilder();
                elementMapBuilder.Add(new DbDeskLampMap()).Add(new DbFridgeMap()).Add(new DbRebarMap());
                desktop.MainWindow = new MainWindow(new MainWindowViewModel(
                    new DbGateway<Models.Product>(config.GetConnectionString("Default")!,
                                                  tableColumnsMap,
                                                  elementMapBuilder.Build())));
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}