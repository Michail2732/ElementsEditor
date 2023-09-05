using ElementsEditor.Gateway.PostgresDb;
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

		public MainWindowViewModel(IElementsGateway<Product> gateway)
		{			
			ElementsEditorViewModel = new ElementsEditorViewModel<Product>(
                gateway,
				pageSize: 40,
				filterFactories: GenerateFilterDescriptors(),
				elementBuilders: GeneratElementBuilders(),
				enableRemoving: true
            );						
        }				

		private IEnumerable<PropertyFilterDescriptor> GenerateFilterDescriptors()
		{
			List<PropertyFilterDescriptor> filters = new List<PropertyFilterDescriptor>();
			filters.Add(new PropertyFilterDescriptor(nameof(Product.Cost), GetCostProperty));
            filters.Add(new PropertyFilterDescriptor(nameof(Product.Name), GetNameProperty));
            filters.Add(new PropertyFilterDescriptor(nameof(Fridge.Temperature), GetTemperatureProperty));
			filters.Add(new PropertyFilterDescriptor(nameof(DeskLamp.Lumen), GetLumenProperty));
            filters.Add(new PropertyFilterDescriptor(nameof(Kettle.Power), GetPowerProperty));
            return filters;
        }


		public IEnumerable<ElementBuilder> GeneratElementBuilders()
		{
			return new List<ElementBuilder>()
			{
				new FridgeBuilder("Create fridge"),
				new KettleBuilder("Create rebar"),
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

        private bool GetPowerProperty(Element element, out int result)
        {
            var kettle = element as Kettle;
            result = kettle?.Power ?? default;
            return kettle is null ? false : true;
        }

        private bool GetLumenProperty(Element element, out int result)
        {
            var deskLamp = element as DeskLamp;
            result = deskLamp?.Lumen ?? default;
            return deskLamp is null ? false : true;
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
