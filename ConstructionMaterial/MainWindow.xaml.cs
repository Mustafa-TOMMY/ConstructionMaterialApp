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
            CalculatorWindow window2 = new CalculatorWindow();
            window2.Show();
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            // Open the Add Material window
            AddMaterialWindow addMaterialWindow = new AddMaterialWindow(_data);
            addMaterialWindow.ShowDialog();

        }
    }
}