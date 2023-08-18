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
        public Product(string id, AccessRights accessRights, decimal cost, string name) : base(id, accessRights)
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
        public Fridge(string id, AccessRights accessRights, decimal cost, string name, int temperature) : base(id, accessRights, cost, name)
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
        public DeskLamp(string id, AccessRights accessRights, decimal cost, string name, int lumen) : base(id, accessRights, cost, name)
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


    public sealed class Rebar : Product
    {
        public Rebar(string id, AccessRights accessRights, decimal cost, string name, string type) : base(id, accessRights, cost, name)
        {
            _type = type;
        }


        private string _type;
        public string Type
        {
            get => _type;
            set => SetAndRaisePropertyChanged(ref _type, value);
        }

    }



}
