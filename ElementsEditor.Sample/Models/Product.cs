using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ElementsEditor;

namespace ElementsEditor.Sample.Models
{
    public class Product : Element
    {
        public Product(string id, decimal cost, string name) : base(id)
        {
            _cost = cost;
            _name = name;
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetAndRaisePropertyChanged(ref _name, value);
        }


        private decimal _cost;
        public decimal Cost
        {
            get => _cost;
            set => SetAndRaisePropertyChanged(ref _cost, value);
        }
    }

    public sealed class Fridge : Product
    {
        public Fridge(string id, decimal cost, string name, int temperature) : base(id,  cost, name)
        {
            _temperature = temperature;
        }

        private int _temperature;
        public int Temperature
        {
            get => _temperature;
            set => SetAndRaisePropertyChanged(ref _temperature, value);
        }
    }

    public sealed class DeskLamp : Product
    {
        public DeskLamp(string id, decimal cost, string name, int lumen) : base(id, cost, name)
        {
            _lumen = lumen;
        }

        private int _lumen;
        public int Lumen
        {
            get => _lumen;
            set => SetAndRaisePropertyChanged(ref _lumen, value);
        }
    }


    public sealed class Kettle : Product
    {
        public Kettle(string id, decimal cost, string name, int power) : base(id, cost, name)
        {
            _power = power;
        }


        private int _power;
        public int Power
        {
            get => _power;
            set => SetAndRaisePropertyChanged(ref _power, value);
        }

    }



}
