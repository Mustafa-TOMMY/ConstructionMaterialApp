using ConstructionMaterial.Helpers;
using ConstructionMaterial.Models;
using ConstructionMaterial.Models.Enum;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConstructionMaterial.UserControls
{
    public partial class ConcreteCalculationTab : UserControl
    {
        /// <summary>
        /// Interaction logic for ConcreteCalculationTab.xaml
        /// </summary>
        public ConcreteCalculationTab()
        {
            InitializeComponent();
            TurnButtons(false);
        }
        public event Action<Order> OnOrderCreated;

        public string OutputConcreteValue
        {
            get { return (string)GetValue(OutputConcreteValueProperty); }
            set { SetValue(OutputConcreteValueProperty, value); }
        }

        public static readonly DependencyProperty OutputConcreteValueProperty =
            DependencyProperty.Register("OutputConcreteValue", typeof(string), typeof(ConcreteCalculationTab), new PropertyMetadata("0.00 m³"));

        private void ConcreteTabCalculateButton_Click(object sender, RoutedEventArgs e)
        {
            double length = Helper.GetNumericalValue(LengthTxt);
            double width = Helper.GetNumericalValue(WidthTxt);
            double depth = Helper.GetNumericalValue(DepthTxt);
            double quantity = Helper.GetNumericalValue(QuantityTxt);
            double volume = length * width * depth * quantity * 1.1;

            OutputConcreteValue = volume.ToString("0.00") + " m³";
        }

        private void ConcreteTabSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaterial = MaterialComboBox.SelectedItem as MainMaterial;
            if (selectedMaterial == null)
            {
                MessageBox.Show("Please select a material first.",
                    "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var order = new Order
            {
                MaterialName = selectedMaterial.Name,
                Category = selectedMaterial.Category,
                Quantity = Helper.GetNumericalValue(QuantityTxt),
                Unit = selectedMaterial.Unit,
                ElementType = (ElementType)ElementTypeComboBox.SelectedItem,
                UnitPrice = selectedMaterial.UnitPrice,
                Status = "Pending",
                Date = DateTime.Now
            };

            OnOrderCreated(order);

            MessageBox.Show("Concrete order saved successfully.");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsInitialized) return;

            if (sender is TextBox textBox)
            {
                string input = textBox.Text.Trim();
                bool isNumber = double.TryParse(input, out double value);

                bool isValid = !string.IsNullOrWhiteSpace(input) && isNumber && value > 0;

                if (!isValid)
                {
                    textBox.BorderBrush = Brushes.Red;
                    textBox.BorderThickness = new Thickness(1.5);
                    textBox.ToolTip = "Please enter a valid number greater than 0";
                }
                else
                {
                    textBox.ClearValue(BorderBrushProperty);
                    textBox.ClearValue(BorderThicknessProperty);
                    textBox.ToolTip = null;
                }
            }

            ToggleButtons();
        }

        private void ToggleButtons()
        {
            bool isAllValid =
                double.TryParse(LengthTxt.Text, out double l) && l > 0 &&
                double.TryParse(WidthTxt.Text, out double w) && w > 0 &&
                double.TryParse(DepthTxt.Text, out double d) && d > 0 &&
                double.TryParse(QuantityTxt.Text, out double q) && q > 0;
            TurnButtons(isAllValid);
        }
        private void TurnButtons(bool isEnabled)
        {
            CalculateBtn.IsEnabled = isEnabled;
            SaveBtn.IsEnabled = isEnabled;
        }
    }
}