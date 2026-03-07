using ConstructionMaterial.Helpers;
using ConstructionMaterial.Models;
using ConstructionMaterial.Models.Enum;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ConstructionMaterial.Views
{
    public partial class OrdersWindow : Window
    {
        public ObservableCollection<Order> Orders { get; set; }
        public ICollectionView OrdersCollection { get; set; }
        public List<MaterialType> SearchOptions { get; set; }
        public List<string> MaterialForSearch { get; set; }
        private AppData _data { get; set; }
        public OrdersWindow(AppData data)
        {
            InitializeComponent();
            _data = data;
            Orders = _data.Orders;
            OrdersCollection = CollectionViewSource.GetDefaultView(Orders);
            SearchOptions = Enum.GetValues<MaterialType>().ToList();
            DataContext = this;
            MaterialForSearch = OrdersCollection.Cast<Order>().Select(o => o.MaterialName).Distinct().ToList();
            UpdateSummary();
        }

        private void ApplyFilter()
        {
            if (OrdersCollection == null) return;

            OrdersCollection.Filter = (item) =>
            {
                if (item is not Order order) return false;
                //var order = item as Order;
                bool categoryMatch = true;
                //if (CategorySearchComboBox.SelectedItem is MaterialType selectedType)
                var selectedType = CategorySearchComboBox.SelectedItem as MaterialType?;
                categoryMatch = order.Category == selectedType;

                bool materialMatch = true;
                if (MaterialSearchComboBox.SelectedItem != null)
                    materialMatch = order.MaterialName == MaterialSearchComboBox.SelectedItem.ToString();

                bool statusMatch = true;
                if (StatusSearchComboBox.SelectedItem is ComboBoxItem statusItem
                    && statusItem.Content.ToString() != "All")
                    statusMatch = order.Status == statusItem.Content.ToString();

                bool dateMatch = true;
                if (FromDatePicker.SelectedDate.HasValue)
                    dateMatch &= order.Date >= FromDatePicker.SelectedDate.Value;
                if (ToDatePicker.SelectedDate.HasValue)
                    dateMatch &= order.Date <= ToDatePicker.SelectedDate.Value;

                return categoryMatch && materialMatch && statusMatch && dateMatch;
            };

            UpdateSummary();
        }

        private void UpdateMaterialList()
        {
            var currentCategory = CategorySearchComboBox.SelectedItem as MaterialType?;

            MaterialForSearch = OrdersCollection
                .Cast<Order>()
                .Where(o => o.Category == currentCategory)
                .Select(o => o.MaterialName)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            MaterialSearchComboBox.ItemsSource = MaterialForSearch;
        }

        private void UpdateSummary()
        {
            if (ItemsCountText == null || TotalCostText == null || PendingText == null) return;

            var filteredOrders = OrdersCollection.Cast<Order>().ToList();

            ItemsCountText.Text = filteredOrders.Count.ToString();
            TotalCostText.Text = $"{filteredOrders.Sum(o => o.Total):N2} EGP";
            PendingText.Text = filteredOrders.Count(o => o.Status == "Pending").ToString();
        }

        private void Filter(object sender, EventArgs e)
        {
            ApplyFilter();
            UpdateMaterialList();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            CategorySearchComboBox.SelectedItem = null;
            MaterialSearchComboBox.SelectedItem = null;
            StatusSearchComboBox.SelectedIndex = 0;
            FromDatePicker.SelectedDate = null;
            ToDatePicker.SelectedDate = null;

            OrdersCollection.Filter = null;
            UpdateMaterialList();
            UpdateSummary();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is not Order selectedOrder) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete Order #{selectedOrder.OrderNumber}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Orders.Remove(selectedOrder);
                Helper.SaveToJson(_data);
                UpdateSummary();
            }
        }

        private void DeliveryMarkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is not Order selectedOrder) return;

            if (selectedOrder.Status == "Delivered")
            {
                MessageBox.Show("This order is already delivered.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            selectedOrder.Status = "Delivered";
            OrdersCollection.Refresh();
            Helper.SaveToJson(_data);
            UpdateSummary();
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                FileName = "Orders.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                var filePath = dialog.FileName;
                while (Path.GetExtension(filePath) != ".csv")
                {
                    MessageBox.Show("Please select a CSV file format only.", "Invalid File Type",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }
                var records = OrdersCollection.Cast<Order>().ToList();
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(records);
                }
                MessageBox.Show("Export completed successfully!",
                    "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}