using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor.Sample.Models
{
    public abstract class ProductBuilder : ElementBuilder
    {
        public ProductBuilder(string name) : base(name)
        {
        }

        private string _productName;
        public string ProductName
        {
            get => _productName;
            set => SetAndRaisePropertyChanged(ref _productName, value);
        }


        private decimal _cost;
        public decimal Cost
        {
            get => _cost;
            set => SetAndRaisePropertyChanged(ref _cost, value);
        }
    }

    public class FridgeBuilder : ProductBuilder
    {
        public FridgeBuilder(string name) : base(name)
        {
        }

        private int _temperature;
        public int Temperature
        {
            get => _temperature;
            set => SetAndRaisePropertyChanged(ref _temperature, value);
        }

        public override Element Build()
        {
            return new Fridge(Guid.NewGuid().ToString(), AccessRights.All, Cost, ProductName, _temperature);
        }

        public override void ResetProperties()
        {
            Cost = default;
            ProductName = string.Empty;
            Temperature = default;
        }
    }


    public class RebarBuilder : ProductBuilder
    {
        public RebarBuilder(string name) : base(name)
        {
        }

        private string? _type;
        public string? Type
        {
            get => _type;
            set => SetAndRaisePropertyChanged(ref _type, value);
        }

        public override Element Build()
        {
            return new Rebar(Guid.NewGuid().ToString(), AccessRights.All, Cost, ProductName, _type);
        }

        public override void ResetProperties()
        {
            Cost = default;
            ProductName = string.Empty;
            Type = string.Empty;
        }
    }

    public class DeskLampBuilder : ProductBuilder
    {
        public DeskLampBuilder(string name) : base(name)
        {
        }

        private int _lumen;
        public int Lumen
        {
            get => _lumen;
            set => SetAndRaisePropertyChanged(ref _lumen, value);
        }

        public override Element Build()
        {
            return new DeskLamp(Guid.NewGuid().ToString(), AccessRights.All, Cost, ProductName, _lumen);
        }

        public override void ResetProperties()
        {
            Cost = default;
            ProductName = string.Empty;
            Lumen = default;
        }
    }    

}
