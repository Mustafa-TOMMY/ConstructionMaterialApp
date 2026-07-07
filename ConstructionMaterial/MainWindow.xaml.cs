using ConstructionMaterial.ViewModels;
using ConstructionMaterial.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace ConstructionMaterial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MainViewModel? _mianViewModel;

        public MainWindow(IServiceProvider serviceProvider, MainViewModel mainViewModel)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider;
            _mianViewModel = mainViewModel;

            DataContext = mainViewModel;

            mainViewModel.Materials.OpenMaterialWindowRequested += OpenMaterialWindow;
            Activated += Window_Activated;
        }

        private void OpenMaterialWindow()
        {
            var window = _serviceProvider.GetRequiredService<AddMaterialWindow>();
            window.ShowDialog();
        }

        #region Main button to open the windows
        private void Calculator_Click(object sender, RoutedEventArgs e)
        {
            var calculatorWindow = _serviceProvider.GetRequiredService<CalculatorWindow>();
            calculatorWindow.Show();
        }

        private void OrderWindow_Click(object sender, RoutedEventArgs e)
        {
            var ordersWindow = _serviceProvider.GetRequiredService<OrdersWindow>();
            ordersWindow.Show();
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            if(_mianViewModel != null)
            _mianViewModel.Materials.ClearForm();
            OpenMaterialWindow();
        }
        #endregion

        #region basic option in main window
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_mianViewModel != null)
            {
                _mianViewModel.LastSaved = $"Last Saved: {DateTime.Now:dd/MM/yyyy HH:mm}";
            }
            MessageBox.Show("All changes are automatically saved to file.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
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
        private void Window_Activated(object? sender, EventArgs e)
        {
            _mianViewModel?.Orders.LoadOrders();
        }
        #endregion

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MyDataGrid.SelectedItem = null;
        }
    }
}