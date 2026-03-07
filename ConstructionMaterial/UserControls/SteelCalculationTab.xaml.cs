using ConstructionMaterial.Helpers;
using ConstructionMaterial.Models;
using ConstructionMaterial.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConstructionMaterial.UserControls
{
    /// <summary>
    /// Interaction logic for SteelCalculationTab.xaml
    /// </summary>
    public partial class SteelCalculationTab : UserControl
    {
        public event Action<Order> OnOrderCreated;

        public SteelCalculationTab()
        {
            InitializeComponent();
            TurnButtons(false);
        }



        public string OutputSteelValue
        {
            get { return (string)GetValue(OutputSteelValueProperty); }
            set { SetValue(OutputSteelValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutputSteelValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutputSteelValueProperty =
            DependencyProperty.Register("OutputSteelValue", typeof(string), typeof(SteelCalculationTab), new PropertyMetadata(""));

        private void SteelTabCalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (BarDiameterComboBox.SelectedItem == null) return;

            double diameter = Convert.ToDouble(BarDiameterComboBox.SelectedItem);
            double length = Helper.GetNumericalValue(BarLengthTxt);
            double bars = Helper.GetNumericalValue(NoOfBarsTxt);

            double weightPerBar = (diameter * diameter / 162.0) * length;
            double totalWeight = weightPerBar * bars;
            double tons = totalWeight / 1000;

            OutputSteelValue = totalWeight.ToString("0.00") + " kg | " + tons.ToString("0.000") + " ton";
        }

        private void SteelTabSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var order = new Order
            {
                MaterialName = "Steel Bars",
                Category = MaterialType.Steel,
                Quantity = Helper.GetNumericalValue(NoOfBarsTxt),
                Unit = "Bars",
                UnitPrice = 0,
                Status = "Pending",
                Date = DateTime.Now
            };
            OnOrderCreated(order);
            MessageBox.Show("Steel order saved successfully.");
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
                double.TryParse(BarLengthTxt.Text, out double bl) && bl > 0 &&
                double.TryParse(NoOfBarsTxt.Text, out double nb) && nb > 0;

            TurnButtons(isAllValid);
        }

        private void TurnButtons(bool isEnabled)
        {
            CalculateBtn.IsEnabled = isEnabled;
            SaveBtn.IsEnabled = isEnabled;
        }
    }
}
