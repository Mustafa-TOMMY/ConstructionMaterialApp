using System.Windows;
using Microsoft.Win32;
using ConstructionMaterial.ViewModels;

namespace ConstructionMaterial.Views
{
    public partial class OrdersWindow : Window
    {
        public OrdersWindow(OrderViewModel orderViewModel)
        {
            InitializeComponent();
            DataContext = orderViewModel;

            // Reload orders to ensure we display the most up-to-date data
            orderViewModel.LoadOrders();

            // Setup the save file dialog delegate for exporting to CSV
            orderViewModel.SaveFilePicker = () =>
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    FileName = "Orders.csv"
                };

                if (dialog.ShowDialog() == true)
                {
                    return dialog.FileName;
                }
                return null;
            };
        }
    }
}