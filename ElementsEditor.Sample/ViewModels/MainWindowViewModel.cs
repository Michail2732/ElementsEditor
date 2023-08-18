using ElementsEditor.Sample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ElementsEditor.Sample.ViewModels
{ 
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		public IElementsGateway ProductGateway { get; }
		public IEnumerable<PropertyFilterCreator> FiltersInfo { get; }

		public MainWindowViewModel()
		{
			ProductGateway = new ElementsCollectionGateway(GenerateProducts())
			{
				DebugDelay = 700
			};
			FiltersInfo = GenerateProductFilters();
        }		


		private IEnumerable<Element> GenerateProducts()
		{
			int count = 1000;
			var products = new List<Product>(count);
			for (int i = 0; i < count; i++)
			{
				if (count % 5 == 0)
					products.Add(new DeskLamp(Guid.NewGuid().ToString(), AccessRights.All, (Decimal)1.24 * i, $"Electros sa{i}", i + 259 / 2));
				else if (count % 3 == 0)
					products.Add(new Rebar(Guid.NewGuid().ToString(), AccessRights.All, (Decimal)1.24 * i, $"Armatura {i}", $"PT1{i%2}-NZ{i}"));
				else if (count % 2 == 0)
					products.Add(new Fridge(Guid.NewGuid().ToString(), AccessRights.All, (Decimal)1.24 * i, $"Indesit LX{i}", -(i % 100 + 13)));				
			}
			return products;
		}

		private IEnumerable<PropertyFilterCreator> GenerateProductFilters()
		{
			List<PropertyFilterCreator> filters = new List<PropertyFilterCreator>();
			filters.Add(new PropertyFilterCreator(nameof(Product.Cost), GetCostProperty));
            filters.Add(new PropertyFilterCreator(nameof(Product.Name), GetNameProperty));
            filters.Add(new PropertyFilterCreator(nameof(Fridge.Temperature), GetTemperatureProperty));
			return filters;
        }


		private bool GetNameProperty(Element element, out string result)
		{
			result = ((Product)element).Name;
			return true;
		}

        private bool GetCostProperty(Element element, out Decimal result)
        {
            result = ((Product)element).Cost;
            return true;
        }

        private bool GetTemperatureProperty(Element element, out int result)
        {
			var fridge = element as Fridge;
            result = fridge?.Temperature ?? default;
            return fridge is null ? false : true;
        }

        #region INotifyPropertyChanged impl
        public event PropertyChangedEventHandler? PropertyChanged;
		protected void SetAndRaisePropertyChanged<T>(ref T oldValue, T newValue,
			[CallerMemberName] string property = "")
		{
			if (oldValue?.Equals(newValue) == true)
				return;
			oldValue = newValue;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}
		#endregion
	}

}
