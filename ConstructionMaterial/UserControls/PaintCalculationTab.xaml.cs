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
    /// Interaction logic for PaintCalculationTab.xaml
    /// </summary>
    public partial class PaintCalculationTab : UserControl
    {
        public event Action<Order> OnOrderCreated;
        public PaintCalculationTab()
        {
            InitializeComponent();
            TurnButtons(false);
        }
        public string OutputPaintValue
        {
            get { return (string)GetValue(OutputPaintValueProperty); }
            set { SetValue(OutputPaintValueProperty, value); }
        }

        public static readonly DependencyProperty OutputPaintValueProperty =
            DependencyProperty.Register("OutputPaintValue", typeof(string), typeof(PaintCalculationTab), new PropertyMetadata("0.00 Liters"));

        private void PaintTabCalculateButton_Click(object sender, RoutedEventArgs e)
        {
            double area = Helper.GetNumericalValue(SurfaceAreaTxt);
            double coats = Helper.GetNumericalValue(NoOfCoatsTxt);
            double coverage = Helper.GetNumericalValue(CoverageRateTxt);

            if (coverage > 0)
            {
                double liters = (area * coats) / coverage;
                OutputPaintValue = liters.ToString("0.00") + " Liters";
            }
            else
            {
                OutputPaintValue = "0.00 Liters";
            }
        }

        private void PaintTabSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var order = new Order
            {
                MaterialName = "Paint",
                Category = MaterialType.Paint,
                Quantity = Helper.GetNumericalValue(SurfaceAreaTxt),
                Unit = "m²",
                UnitPrice = 0,
                Status = "Pending",
                Date = DateTime.Now
            };

            OnOrderCreated(order);

            MessageBox.Show("Paint order saved successfully.");
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
                double.TryParse(SurfaceAreaTxt.Text, out double sa) && sa > 0 &&
                double.TryParse(NoOfCoatsTxt.Text, out double nc) && nc > 0 &&
                double.TryParse(CoverageRateTxt.Text, out double cr) && cr > 0;

            TurnButtons(isAllValid);
        }

        private void TurnButtons(bool isEnabled)
        {
            CalculateBtn.IsEnabled = isEnabled;
            SaveBtn.IsEnabled = isEnabled;
        }
    }
}
