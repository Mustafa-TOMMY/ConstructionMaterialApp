using ConstructionMaterial.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionMaterial.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MaterialViewModel Materials { get; }
        public OrderViewModel Orders { get; }

        public MainViewModel(MaterialViewModel materials, OrderViewModel orders)
        {
            Materials = materials;
            Orders = orders;
        }
    }
}
