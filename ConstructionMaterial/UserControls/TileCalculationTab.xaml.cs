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
    /// Interaction logic for TileCalculationTab.xaml
    /// </summary>
    public partial class TileCalculationTab : UserControl
    {
        public event Action<Order> OnOrderCreated;
        public TileCalculationTab()
        {
            InitializeComponent();
            TurnButtons(false);
        }
        public string OutputTilesValue
        {
            get { return (string)GetValue(OutputTilesValueProperty); }
            set { SetValue(OutputTilesValueProperty, value); }
        }

        public static readonly DependencyProperty OutputTilesValueProperty =
            DependencyProperty.Register("OutputTilesValue", typeof(string), typeof(TileCalculationTab), new PropertyMetadata("0 Tiles"));

        private void TilesTabCalculateButton_Click(object sender, RoutedEventArgs e)
        {
            double length = Helper.GetNumericalValue(RoomLengthTxt);
            double width = Helper.GetNumericalValue(RoomWidthTxt);
            double waste = Helper.GetNumericalValue(WasteTxt);

            if (TileSizeComboBox.SelectedItem is Tile selectedTile)
            {
                double roomArea = length * width;
                double tileArea = selectedTile.Size;

                if (tileArea > 0)
                {
                    double totalTiles = (roomArea / tileArea) * (1 + waste / 100);
                    OutputTilesValue = Math.Ceiling(totalTiles).ToString() + " Tiles";
                }
            }
            else
            {
                MessageBox.Show("Please select a tile size.");
            }
        }

        private void TilesTabSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var order = new Order
            {
                MaterialName = "Tiles",
                Category = MaterialType.Tiles,
                Quantity = Helper.GetNumericalValue(RoomLengthTxt) * Helper.GetNumericalValue(RoomWidthTxt),
                Unit = "m²",
                UnitPrice = 0,
                Status = "Pending",
                Date = DateTime.Now
            };
            OnOrderCreated?.Invoke(order);
            MessageBox.Show("Tiles order saved successfully.");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsInitialized) return;

            if (sender is TextBox textBox)
            {
                string input = textBox.Text.Trim();
                bool isNumber = double.TryParse(input, out double value);
                bool isValid;
                if (textBox.Name == "WasteTxt")
                    isValid = !string.IsNullOrWhiteSpace(input) && isNumber && value >= 0;
                else
                    isValid = !string.IsNullOrWhiteSpace(input) && isNumber && value > 0;

                if (!isValid)
                {
                    textBox.BorderBrush = Brushes.Red;
                    textBox.BorderThickness = new Thickness(1.5);
                    textBox.ToolTip = "Please enter a valid positive number";
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
                double.TryParse(RoomLengthTxt.Text, out double rl) && rl > 0 &&
                double.TryParse(RoomWidthTxt.Text, out double rw) && rw > 0 &&
                double.TryParse(WasteTxt.Text, out double wa) && wa >= 0;

            TurnButtons(isAllValid);
        }

        private void TurnButtons(bool isEnabled)
        {
            CalculateBtn.IsEnabled = isEnabled;
            SaveBtn.IsEnabled = isEnabled;
        }
    }
}
