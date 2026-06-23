using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using CsvHelper;
using CsvHelper.Configuration;
using ConstructionMaterial.DAL.Models.Enum;
using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using ConstructionMaterial.Core;

namespace ConstructionMaterial.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly IOrderService _orderService;
        private ObservableCollection<OrderDto> _orders = new();
        private ICollectionView? _ordersCollection;
        private List<MaterialType> _searchOptions = new();
        private List<string> _materialForSearch = new();
        private MaterialType? _selectedCategory;
        private string? _selectedMaterial;
        private string _selectedStatus = "All";
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private OrderDto? _selectedOrder;
        private int _itemsCount;
        private double _totalCost;
        private int _pendingCount;

        public ObservableCollection<OrderDto> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(TotalPendingCount));
            }
        }

        public ICollectionView? OrdersCollection
        {
            get => _ordersCollection;
            set
            {
                _ordersCollection = value;
                OnPropertyChanged();
            }
        }

        public List<MaterialType> SearchOptions
        {
            get => _searchOptions;
            set
            {
                _searchOptions = value;
                OnPropertyChanged();
            }
        }

        public List<string> StatusOptions { get; } = new() { "All", "Pending", "Delivered" };

        public List<string> MaterialForSearch
        {
            get => _materialForSearch;
            set
            {
                _materialForSearch = value;
                OnPropertyChanged();
            }
        }

        public MaterialType? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                    SelectedMaterial = null;
                    UpdateMaterialList();
                    ApplyFilter();
                }
            }
        }

        public string? SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                if (_selectedMaterial != value)
                {
                    _selectedMaterial = value;
                    OnPropertyChanged();
                    ApplyFilter();
                }
            }
        }

        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged();
                    ApplyFilter();
                }
            }
        }

        public DateTime? FromDate
        {
            get => _fromDate;
            set
            {
                if (_fromDate != value)
                {
                    _fromDate = value;
                    OnPropertyChanged();
                    ApplyFilter();
                }
            }
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                if (_toDate != value)
                {
                    _toDate = value;
                    OnPropertyChanged();
                    ApplyFilter();
                }
            }
        }

        public OrderDto? SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    OnPropertyChanged();
                    (DeleteCommand as BaseCommand)?.RaiseCanExecuteChanged();
                    (MarkDeliveredCommand as BaseCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public int ItemsCount
        {
            get => _itemsCount;
            set
            {
                _itemsCount = value;
                OnPropertyChanged();
            }
        }

        public double TotalCost
        {
            get => _totalCost;
            set
            {
                _totalCost = value;
                OnPropertyChanged();
            }
        }

        public int PendingCount
        {
            get => _pendingCount;
            set
            {
                _pendingCount = value;
                OnPropertyChanged();
            }
        }

        public int Count => Orders?.Count ?? 0;
        public int TotalPendingCount => Orders?.Count(o => o.Status == "Pending") ?? 0;

        public Func<string?>? SaveFilePicker { get; set; }

        public ICommand ResetCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand MarkDeliveredCommand { get; }
        public ICommand ExportCommand { get; }

        public OrderViewModel(IOrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));

            SearchOptions = Enum.GetValues<MaterialType>().ToList();

            ResetCommand = new BaseCommand(ResetFilters);
            DeleteCommand = new BaseCommand(p => DeleteOrder(), p => SelectedOrder != null);
            MarkDeliveredCommand = new BaseCommand(p => MarkOrderDelivered(), p => SelectedOrder != null);
            ExportCommand = new BaseCommand(ExportOrders);

            LoadOrders();
        }

        public void LoadOrders()
        {
            var list = _orderService.GetAllOrders();
            Orders = new ObservableCollection<OrderDto>(list);
            OrdersCollection = CollectionViewSource.GetDefaultView(Orders);

            UpdateMaterialList();
            ApplyFilter();
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(TotalPendingCount));
        }

        private void ApplyFilter()
        {
            if (OrdersCollection == null) return;

            OrdersCollection.Filter = (item) =>
            {
                if (item is not OrderDto order) return false;

                bool categoryMatch = true;
                if (SelectedCategory.HasValue)
                {
                    categoryMatch = order.Category == SelectedCategory.Value.ToString();
                }

                bool materialMatch = true;
                if (!string.IsNullOrEmpty(SelectedMaterial))
                {
                    materialMatch = order.MaterialName == SelectedMaterial;
                }

                bool statusMatch = true;
                if (!string.IsNullOrEmpty(SelectedStatus) && SelectedStatus != "All")
                {
                    statusMatch = order.Status == SelectedStatus;
                }

                bool dateMatch = true;
                if (FromDate.HasValue)
                {
                    dateMatch &= order.Date >= FromDate.Value;
                }
                if (ToDate.HasValue)
                {
                    dateMatch &= order.Date <= ToDate.Value;
                }

                return categoryMatch && materialMatch && statusMatch && dateMatch;
            };

            UpdateSummary();
        }

        private void UpdateMaterialList()
        {
            if (SelectedCategory.HasValue)
            {
                MaterialForSearch = Orders
                    .Where(o => o.Category == SelectedCategory.Value.ToString())
                    .Select(o => o.MaterialName)
                    .Distinct()
                    .OrderBy(name => name)
                    .ToList();
            }
            else
            {
                MaterialForSearch = Orders
                    .Select(o => o.MaterialName)
                    .Distinct()
                    .OrderBy(name => name)
                    .ToList();
            }
        }

        private void UpdateSummary()
        {
            if (OrdersCollection == null) return;

            var filteredOrders = OrdersCollection.Cast<OrderDto>().ToList();

            ItemsCount = filteredOrders.Count;
            TotalCost = filteredOrders.Sum(o => o.Total);
            PendingCount = filteredOrders.Count(o => o.Status == "Pending");
        }

        private void ResetFilters()
        {
            _selectedCategory = null;
            _selectedMaterial = null;
            _selectedStatus = "All";
            _fromDate = null;
            _toDate = null;

            OnPropertyChanged(nameof(SelectedCategory));
            OnPropertyChanged(nameof(SelectedMaterial));
            OnPropertyChanged(nameof(SelectedStatus));
            OnPropertyChanged(nameof(FromDate));
            OnPropertyChanged(nameof(ToDate));

            if (OrdersCollection != null)
            {
                OrdersCollection.Filter = null;
            }
            UpdateMaterialList();
            UpdateSummary();
        }

        private void DeleteOrder()
        {
            if (SelectedOrder == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete Order #{SelectedOrder.OrderNumber}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _orderService.RemoveOrder(SelectedOrder.Id);
                    Orders.Remove(SelectedOrder);
                    SelectedOrder = null;
                    UpdateSummary();
                    OnPropertyChanged(nameof(Count));
                    OnPropertyChanged(nameof(TotalPendingCount));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MarkOrderDelivered()
        {
            if (SelectedOrder == null) return;

            if (SelectedOrder.Status == "Delivered")
            {
                MessageBox.Show("This order is already delivered.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                SelectedOrder.Status = "Delivered";
                _orderService.UpdateOrder(SelectedOrder);
                OrdersCollection?.Refresh();
                UpdateSummary();
                OnPropertyChanged(nameof(TotalPendingCount));
                (MarkDeliveredCommand as BaseCommand)?.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportOrders()
        {
            if (SaveFilePicker == null) return;

            var filePath = SaveFilePicker();
            if (string.IsNullOrEmpty(filePath)) return;

            if (Path.GetExtension(filePath).ToLower() != ".csv")
            {
                MessageBox.Show("Please select a CSV file format only.", "Invalid File Type",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (OrdersCollection == null) return;

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
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting orders: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}