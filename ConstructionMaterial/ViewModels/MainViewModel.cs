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

        public BaseCommand SaveCommand { get; }
        public BaseCommand LoadCommand { get; }

        public MainViewModel(MaterialViewModel materials, OrderViewModel orders)
        {
            Materials = materials;
            Orders = orders;

            SaveCommand = new BaseCommand(p => Save(), p => true);
            LoadCommand = new BaseCommand(p => Load(), p => true);

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

        private void Save()
        {
            LastSaved = $"Last Saved: {DateTime.Now:dd/MM/yyyy HH:mm}";
            System.Windows.MessageBox.Show("All changes are automatically saved to file.", "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void Load()
        {
            Materials.GetMaterials();
            Orders.LoadOrders();
            LastSaved = $"Last Loaded: {DateTime.Now:dd/MM/yyyy HH:mm}";
            System.Windows.MessageBox.Show("Data successfully reloaded from database.", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
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
