using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ElementsEditor.Gateway.PostgresDb;
using ElementsEditor.Sample.Configuration;
using ElementsEditor.Sample.Helpers;
using ElementsEditor.Sample.Models;
using ElementsEditor.Sample.PostgresDb;
using ElementsEditor.Sample.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

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
                desktop.MainWindow = new MainWindow(new MainWindowViewModel(GetGateway()));
            }

            base.OnFrameworkInitializationCompleted();
        }

        private IElementsGateway<Product> GetGateway()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            if (config["DataProvider"] == "Postgres")
                return GatewaysFactory.CreatePostgres(config);
            else
                return GatewaysFactory.CreateInMemory(config);
        }       
    }
}