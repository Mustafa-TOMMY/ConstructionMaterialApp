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
        private string _lastSaved = "Last Saved: --";

        public MaterialViewModel Materials { get; }
        public OrderViewModel Orders { get; }

        public string LastSaved
        {
            get => _lastSaved;
            set
            {
                _lastSaved = value;
                OnPropertyChanged();
            }
        }

        public string TotalCost => $"{Orders.Orders.Sum(o => o.Total):N2} EGP";

        public MainViewModel(MaterialViewModel materials, OrderViewModel orders)
        {
            Materials = materials;
            Orders = orders;

            // Subscribe to updates to update TotalCost on the Main Dashboard
            Orders.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(OrderViewModel.Orders))
                {
                    OnPropertyChanged(nameof(TotalCost));
                    SubscribeToCollectionChanges();
                }
            };
            SubscribeToCollectionChanges();
        }

        private void SubscribeToCollectionChanges()
        {
            if (Orders.Orders != null)
            {
                Orders.Orders.CollectionChanged += (s, e) => {
                    OnPropertyChanged(nameof(TotalCost));
                };
            }
        }
    }
}
