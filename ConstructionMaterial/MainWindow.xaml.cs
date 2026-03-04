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
        private AppData _data;
        public MainWindow()
        {
            InitializeComponent();
            _data = Helper.LoadFromJson();

            MaterialCatalog = _data.Materials;
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CalculatorWindow orderWindow = new CalculatorWindow(_data);
            orderWindow.Show();
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            AddMaterialWindow addMaterialWindow = new AddMaterialWindow(_data);
            addMaterialWindow.ShowDialog();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}