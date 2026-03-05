using ConstructionMaterial.Views;
using ConstructionMaterial.Models;
using System.Collections.ObjectModel;
using System.Windows;
using ConstructionMaterial.Helpers;

namespace ConstructionMaterial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<MainMaterial> MaterialCatalog { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
        private AppData _data;

        public MainWindow()
        {
            InitializeComponent();
            _data = Helper.LoadFromJson();
            StatusBarControl.UpdateLastSaved();
            MaterialCatalog = _data.Materials;
            Orders = _data.Orders;
            DataContext = this;
        }

        private void Calculator_Click(object sender, RoutedEventArgs e)
        {
            CalculatorWindow orderWindow = new CalculatorWindow(_data);
            orderWindow.Show();
        }
        private void OrderWindow_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow orderWindow = new OrdersWindow(_data);
            orderWindow.Show();
        }
        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            AddMaterialWindow addMaterialWindow = new AddMaterialWindow(_data);
            addMaterialWindow.ShowDialog();

        }
        private void DeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaterial = MyDataGrid.SelectedItem as MainMaterial;

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
                _data.Materials.Remove(selectedMaterial);
                Helper.SaveToJson(_data);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Helper.SaveToJson(_data);
            StatusBarControl.UpdateLastSaved();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}