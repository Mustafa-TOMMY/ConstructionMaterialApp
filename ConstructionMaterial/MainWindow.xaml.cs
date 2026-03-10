using ConstructionMaterial.Helpers;
using ConstructionMaterial.Models;
using ConstructionMaterial.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace ConstructionMaterial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<MainMaterial> MaterialCatalog { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
        private AppData _data;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _totalCost;
        public string TotalCost
        {
            get => _totalCost;
            set
            {
                _totalCost = value;
                OnPropertyChanged(nameof(TotalCost));
            }
        }
        private int _orderPending;
        public int OrderPending
        {
            get => _orderPending;
            set
            {
                _orderPending = value;
                OnPropertyChanged(nameof(OrderPending));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            _data = Helper.LoadFromJson();
            StatusBarControl.UpdateLastSaved();
            MaterialCatalog = _data.Materials;
            Orders = _data.Orders;
            TotalCost = $"EGP {Orders.Sum(p => p.Total)}" ?? "0";
            OrderPending = Orders.Count(p => p.Status == "Pending");
            Orders.CollectionChanged += (s, e) =>
            {
                TotalCost = $"EGP {Orders.Sum(p => p.Total)}";
                OrderPending = Orders.Count(p => p.Status == "Pending");
            };
            DataContext = this;
        }

        private void Calculator_Click(object sender, RoutedEventArgs e)
        {
            CalculatorWindow orderWindow = new CalculatorWindow(this);
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
            MessageBoxResult result = MessageBox.Show("Do you want to save changes before exiting?",
                                                    "Confirm Exit",
                                                    MessageBoxButton.YesNoCancel,
                                                    MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SaveData();
            }
            else if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StatusBarControl.UpdateLastSaved();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Window_Closing(sender, new CancelEventArgs());
        }
        public void SaveData()
        {
            Helper.SaveToJson(_data);
            StatusBarControl.UpdateLastSaved();
            TotalCost = $"EGP {Orders.Sum(p => p.Total)}";
        }

        private void UpdateMaterial_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaterial = MyDataGrid.SelectedItem as MainMaterial;
            if (selectedMaterial == null)
            {
                MessageBox.Show("Please select a material to update.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            UpdateMaterialWindow updateWindow = new UpdateMaterialWindow(_data, selectedMaterial);

            if (updateWindow.ShowDialog() == true)
            {
                MyDataGrid.Items.Refresh();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string developerName = "Mustafa Abdelhamid Mustafa Abdelhamid";

            MessageBox.Show($"Developed and Designed by: {developerName}",
                            "About Developer",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
    }
}