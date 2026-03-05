using ConstructionMaterial.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace ConstructionMaterial.Views
{
    /// <summary>
    /// Interaction logic for OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        public ObservableCollection<Order> Orders { get; set; }
        public AppData _data { get; set; }
        public OrdersWindow(AppData data)
        {
            InitializeComponent();
            _data = data;
            Orders = _data.Orders;
            DataContext = this;
        }
    }
}
