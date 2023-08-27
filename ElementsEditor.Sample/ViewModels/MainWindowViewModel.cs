using ElementsEditor.Sample.Models;
using System;
using System.Collections;
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
		public ElementsEditorViewModel<Product> ElementsEditorViewModel { get;}		

		public MainWindowViewModel()
		{
			var products = GenerateElements();
			ElementsEditorViewModel = new ElementsEditorViewModel<Product>(
                new ElementsCollectionGateway<Product>(products)
                {
                    DebugDelay = 700
                },
				pageSize: 40,
				filterFactories: GenerateFilterDescriptors(),
				elementBuilders: GeneratElementBuilders(),
				enableRemoving: true
            );						
        }		


		private IEnumerable<Product> GenerateElements()
		{
			int count = 1000;
			var products = new List<Product>(count);
			for (int i = 0; i < count; i++)
			{
				if (i % 5 == 0)
					products.Add(new DeskLamp(Guid.NewGuid().ToString(), AccessRights.All, (Decimal)1.24 * i, $"Electros sa-{i}", i + 259 / 2));
				else if (i % 3 == 0)
					products.Add(new Rebar(Guid.NewGuid().ToString(), AccessRights.All, (Decimal)1.24 * i, $"A-1(240)-{i}", $"PT1{i % 2}-NZ{i}"));
				else if (i % 2 == 0)
					products.Add(new Fridge(Guid.NewGuid().ToString(), AccessRights.All, (Decimal)1.24 * i, $"Indesit LX-{i}", -(i % 100 + 13)));
				else
					products.Add(new Rebar(Guid.NewGuid().ToString(), AccessRights.All, (Decimal)1.24 * i, $"A-1(300)-{i}", $"HT1{i % 2}-PR{i}"));
            }
			return products;
		}

		private IEnumerable<PropertyFilterDescriptor> GenerateFilterDescriptors()
		{
			List<PropertyFilterDescriptor> filters = new List<PropertyFilterDescriptor>();
			filters.Add(new PropertyFilterDescriptor(nameof(Product.Cost), GetCostProperty));
            filters.Add(new PropertyFilterDescriptor(nameof(Product.Name), GetNameProperty));
            filters.Add(new PropertyFilterDescriptor(nameof(Fridge.Temperature), GetTemperatureProperty));
			return filters;
        }


		public IEnumerable<ElementBuilder> GeneratElementBuilders()
		{
			return new List<ElementBuilder>()
			{
				new FridgeBuilder("Create fridge"),
				new RebarBuilder("Create rebar"),
				new DeskLampBuilder("Create desklamp")
			};
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
