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
            set
            {
                SetAndRaisePropertyChanged(ref _productName, value);
                UpdateCanBuild();
            }
        }


        private decimal _cost;
        public decimal Cost
        {
            get => _cost;
            set => SetAndRaisePropertyChanged(ref _cost, value);
        }

        private void UpdateCanBuild()
        {
            CanBuild = !string.IsNullOrEmpty(ProductName);
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
            return new Fridge(Guid.NewGuid().ToString(), Cost, ProductName, _temperature);
        }

        public override void ResetProperties()
        {
            Cost = default;
            ProductName = string.Empty;
            Temperature = default;
        }


    }


    public class KettleBuilder : ProductBuilder
    {
        public KettleBuilder(string name) : base(name)
        {
        }

        private int _power;
        public int Power
        {
            get => _power;
            set => SetAndRaisePropertyChanged(ref _power, value);
        }

        public override Element Build()
        {
            return new Kettle(Guid.NewGuid().ToString(), Cost, ProductName, _power);
        }

        public override void ResetProperties()
        {
            Cost = default;
            ProductName = string.Empty;
            Power = default;
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
            return new DeskLamp(Guid.NewGuid().ToString(), Cost, ProductName, _lumen);
        }

        public override void ResetProperties()
        {
            Cost = default;
            ProductName = string.Empty;
            Lumen = default;
        }
    }    

}
