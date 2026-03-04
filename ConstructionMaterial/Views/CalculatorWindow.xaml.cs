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
        public List<string> MaterialNames { get; set; }
        public AppData _data { get; set; }

        public CalculatorWindow(AppData data)
        {
            InitializeComponent();
            _data = data;
            ElementTypes = Enum.GetValues(typeof(ElementType)).Cast<ElementType>().ToList();
            MaterialNames = data.Materials.Where(m => m.Category == MaterialType.Concrete).Select(m => m.Name).ToList();
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
            //var newOrder = new Order
            //{
            //    OrderNumber = Helper.GetNumericalValue(OrderNumberTxt),
            //    MaterialName = (MaterialType)MaterialComboBox.SelectedItem,
            //    Category = CategoryTxt.Text,
            //    Quantity = Helper.GetNumericalValue(QuantityTxt),
            //    Unit = UnitTxt.Text,
            //    UnitPrice = Helper.GetNumericalValue(UnitPriceTxt),
            //    Status = "Pending",
            //    Date = DateTime.Now
            //};
            //_data.Orders.Add(newOrder);
            Helper.SaveToJson(_data);
        }
    }
}
