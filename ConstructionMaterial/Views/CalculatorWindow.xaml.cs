using ConstructionMaterial.Helpers;
using ConstructionMaterial.Models;
using ConstructionMaterial.Models.Enum;
using System.Windows;
using System.Windows.Controls;

namespace ConstructionMaterial.Views
{
    /// <summary>
    /// Interaction logic for CalculatorWindow.xaml
    /// </summary>
    public partial class CalculatorWindow : Window
    {
        public List<ElementType> ElementTypes { get; set; }
        public List<MainMaterial> MaterialNames { get; set; }
        public AppData _data { get; set; }

        public CalculatorWindow(AppData data)
        {
            InitializeComponent();
            _data = data;
            ElementTypes = Enum.GetValues(typeof(ElementType)).Cast<ElementType>().ToList();
            MaterialNames = data.Materials.Where(m => m.Category == MaterialType.Concrete).ToList();
            DataContext = this;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            double length = Helper.GetNumericalValue(LengthTxt);
            double width = Helper.GetNumericalValue(WidthTxt);
            double depth = Helper.GetNumericalValue(DepthTxt);
            double quantity = Helper.GetNumericalValue(QuantityTxt);

            TotalOrderValue.Text = (length * width * depth * quantity * 1.1).ToString() + " m³";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaterial = MaterialComboBox.SelectedItem as MainMaterial;

            if (selectedMaterial == null)
            {
                MessageBox.Show("Please select a material from the list first.",
                                "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var newOrder = new Order
            {
                OrderNumber = _data.Orders.Count + 1,
                MaterialName = selectedMaterial.Name,
                Category = selectedMaterial.Category,
                Quantity = Helper.GetNumericalValue(QuantityTxt),
                Unit = selectedMaterial.Unit,
                UnitPrice = selectedMaterial.UnitPrice,
                Status = "Pending",
                Date = DateTime.Now
            };
            _data.Orders.Add(newOrder);
            Helper.SaveToJson(_data);

            MessageBox.Show($"Order for {selectedMaterial.Name} has been saved!",
                            "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
