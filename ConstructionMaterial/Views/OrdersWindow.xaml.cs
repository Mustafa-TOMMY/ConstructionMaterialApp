using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using CsvHelper;
using CsvHelper.Configuration;
using ConstructionMaterial.DAL.Models.Enum;
using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;

namespace ConstructionMaterial.Views
{
    public partial class OrdersWindow : Window
    {
        public ObservableCollection<OrderDto> Orders { get; set; }
        public ICollectionView OrdersCollection { get; set; }
        public List<MaterialType> SearchOptions { get; set; }
        public List<string> MaterialForSearch { get; set; }
        private readonly IOrderService _orderService;

        public OrdersWindow(IOrderService orderService)
        {
            InitializeComponent();
            _orderService = orderService;
            
            var ordersList = _orderService.GetAllOrders();
            Orders = new ObservableCollection<OrderDto>(ordersList);
            
            OrdersCollection = CollectionViewSource.GetDefaultView(Orders);
            SearchOptions = Enum.GetValues<MaterialType>().ToList();
            DataContext = this;
            MaterialForSearch = OrdersCollection.Cast<OrderDto>().Select(o => o.MaterialName).Distinct().ToList();
            UpdateSummary();
        }

        private void ApplyFilter()
        {
            if (OrdersCollection == null) return;

            OrdersCollection.Filter = (item) =>
            {
                if (item is not OrderDto order) return false;
                
                bool categoryMatch = true;
                var selectedType = CategorySearchComboBox.SelectedItem as MaterialType?;
                if (selectedType.HasValue)
                {
                    categoryMatch = order.Category == selectedType.Value.ToString();
                }

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

            if (currentCategory.HasValue)
            {
                MaterialForSearch = OrdersCollection
                    .Cast<OrderDto>()
                    .Where(o => o.Category == currentCategory.Value.ToString())
                    .Select(o => o.MaterialName)
                    .Distinct()
                    .OrderBy(name => name)
                    .ToList();
            }
            else
            {
                MaterialForSearch = OrdersCollection
                    .Cast<OrderDto>()
                    .Select(o => o.MaterialName)
                    .Distinct()
                    .OrderBy(name => name)
                    .ToList();
            }

            MaterialSearchComboBox.ItemsSource = MaterialForSearch;
        }

        private void UpdateSummary()
        {
            if (ItemsCountText == null || TotalCostText == null || PendingText == null) return;

            var filteredOrders = OrdersCollection.Cast<OrderDto>().ToList();

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
            if (OrdersDataGrid.SelectedItem is not OrderDto selectedOrder) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete Order #{selectedOrder.OrderNumber}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _orderService.RemoveOrder(selectedOrder.Id);
                    Orders.Remove(selectedOrder);
                    UpdateSummary();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeliveryMarkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is not OrderDto selectedOrder) return;

            if (selectedOrder.Status == "Delivered")
            {
                MessageBox.Show("This order is already delivered.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                selectedOrder.Status = "Delivered";
                _orderService.UpdateOrder(selectedOrder);
                OrdersCollection.Refresh();
                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
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
                var records = OrdersCollection.Cast<OrderDto>().ToList();
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