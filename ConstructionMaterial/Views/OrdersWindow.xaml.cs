using ConstructionMaterial.Models;
using ConstructionMaterial.Models.Enum;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ConstructionMaterial.Views
{
    /// <summary>
    /// Interaction logic for OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        public ObservableCollection<Order> Orders { get; set; }
        public List<MaterialType> SearchOptions { get; set; }
        public AppData _data { get; set; }
        public OrdersWindow(AppData data)
        {
            InitializeComponent();
            _data = data;
            Orders = _data.Orders;
            SearchOptions = Enum.GetValues<MaterialType>().ToList();
            DataContext = this;
        }

        private void SearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorySearchComboBox.SelectedItem is MaterialType selectedType)
            {
                var filteredData = _data.Orders.Where(o => o.Category == selectedType);
                Orders = new ObservableCollection<Order>(filteredData);
                OrdersDataGrid.ItemsSource = Orders;
            }
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeliveryMarkBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
