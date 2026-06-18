using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ConstructionMaterial.Views;
using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using ConstructionMaterial.ViewModels;

namespace ConstructionMaterial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<MaterialDto> MaterialCatalog { get; set; }
        public ObservableCollection<OrderDto> Orders { get; set; }
        public string TotalCost { get; set; }

        private readonly IMaterialService _materialService;
        private readonly IOrderService _orderService;
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider, IMaterialService materialService, IOrderService orderService,
                            MaterialViewModel materialViewModel)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _materialService = materialService;
            _orderService = orderService;
            

            MaterialCatalog = new ObservableCollection<MaterialDto>(_materialService.GetAllMaterial());
            Orders = new ObservableCollection<OrderDto>(_orderService.GetAllOrders());
            TotalCost = $"EGP {Orders.Sum(p => p.Total):N2}";
            
            DataContext = materialViewModel;
            StatusBarControl.UpdateLastSaved();
        }

        private void Calculator_Click(object sender, RoutedEventArgs e)
        {
            var calculatorWindow = _serviceProvider.GetRequiredService<CalculatorWindow>();
            calculatorWindow.Closed += (s, args) => RefreshData();
            calculatorWindow.Show();
        }

        private void OrderWindow_Click(object sender, RoutedEventArgs e)
        {
            var ordersWindow = _serviceProvider.GetRequiredService<OrdersWindow>();
            ordersWindow.Closed += (s, args) => RefreshData();
            ordersWindow.Show();
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            var addMaterialWindow = _serviceProvider.GetRequiredService<AddMaterialWindow>();
            addMaterialWindow.Closed += (s, args) => RefreshData();
            addMaterialWindow.ShowDialog();
        }

        private void DeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaterial = MyDataGrid.SelectedItem as MaterialDto;

            if (selectedMaterial == null)
            {
                MessageBox.Show("Please select a material to delete.",
                                "No Selection",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                return;
            }
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedMaterial.Name}?",
                                                        "Confirm Deletion",
                                                        MessageBoxButton.YesNo,
                                                        MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _materialService.RemoveMaterial(selectedMaterial.Id);
                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting material: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RefreshData()
        {
            MaterialCatalog.Clear();
            foreach (var m in _materialService.GetAllMaterial())
            {
                MaterialCatalog.Add(m);
            }

            Orders.Clear();
            foreach (var o in _orderService.GetAllOrders())
            {
                Orders.Add(o);
            }

            TotalCost = $"EGP {Orders.Sum(p => p.Total):N2}";
            
            DataContext = null;
            DataContext = this;
            
            StatusBarControl.UpdateLastSaved();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StatusBarControl.UpdateLastSaved();
            MessageBox.Show("All changes are automatically saved to file.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}