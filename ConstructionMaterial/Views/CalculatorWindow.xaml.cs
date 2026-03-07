using ConstructionMaterial.Helpers;
using ConstructionMaterial.Models;
using ConstructionMaterial.Models.Enum;
using System.Windows;

namespace ConstructionMaterial.Views
{
    /// <summary>
    /// Interaction logic for CalculatorWindow.xaml
    /// </summary>
    public partial class CalculatorWindow : Window
    {
        public List<ElementType> ElementTypes { get; set; }
        public List<BarDiameter> BarDiameters { get; set; }
        public List<MainMaterial> MaterialNames { get; set; }
        public List<SurfaceType> SurfaceTypes { get; set; }
        public List<Tile> TileSizes { get; set; }
        public AppData _data { get; set; }

        public CalculatorWindow(AppData data)
        {
            InitializeComponent();
            _data = data;
            ElementTypes = Enum.GetValues(typeof(ElementType)).Cast<ElementType>().ToList();
            BarDiameters = Enum.GetValues(typeof(BarDiameter)).Cast<BarDiameter>().ToList();
            SurfaceTypes = Enum.GetValues(typeof(SurfaceType)).Cast<SurfaceType>().ToList();
            MaterialNames = data.Materials.Where(m => m.Category == MaterialType.Concrete).ToList();
            TileSizes = new List<Tile>
            {
                new Tile { Name = "30x30", Size = 0.09 },
                new Tile { Name = "50x50", Size = 0.1 },
                new Tile { Name = "60x60", Size = 0.12 },
                new Tile { Name = "80x80", Size = 0.64 }
            };
            DataContext = this;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == ConcreteCalculateButton)
                CalculateConcrete();

            else if (sender == SteelCalculateButton)
                CalculateSteel();

            else if (sender == PaintCalculateButton)
                CalculatePaint();

            else if (sender == TilesCalculateButton)
                CalculateTiles();
        }
        private void CalculateConcrete()
        {
            double length = Helper.GetNumericalValue(LengthTxt);
            double width = Helper.GetNumericalValue(WidthTxt);
            double depth = Helper.GetNumericalValue(DepthTxt);
            double quantity = Helper.GetNumericalValue(QuantityTxt);

            double volume = length * width * depth * quantity * 1.1;

            OutputControl.OutputValue = volume.ToString("0.00") + " m³";
        }
        private void CalculateSteel()
        {
            double diameter = Convert.ToDouble(BarDiameterComboBox.SelectedItem);
            double length = Helper.GetNumericalValue(BarLengthTxt);
            double bars = Helper.GetNumericalValue(NoOfBarsTxt);

            double weightPerBar = (diameter * diameter / 162) * length;

            double totalWeight = weightPerBar * bars;

            double tons = totalWeight / 1000;

            OutputSteelControl.OutputValue = totalWeight.ToString("0.00") + " kg | " + tons.ToString("0.000") + " ton";
        }
        private void CalculatePaint()
        {
            double area = Helper.GetNumericalValue(SurfaceAreaTxt);
            double coats = Helper.GetNumericalValue(NoOfCoatsTxt);
            double coverage = Helper.GetNumericalValue(CoverageRateTxt);

            double liters = (area * coats) / coverage;

            OutputPaintControl.OutputValue = liters.ToString("0.00") + " Liters";
        }
        private void CalculateTiles()
        {
            double length = Helper.GetNumericalValue(RoomLengthTxt);
            double width = Helper.GetNumericalValue(RoomWidthTxt);
            double waste = Helper.GetNumericalValue(WasteTxt);

            double roomArea = length * width;

            double tileSize = (TileSizeComboBox.SelectedItem as Tile).Size;

            double tileArea = tileSize;

            double tiles = (roomArea / tileArea) * (1 + waste / 100);

            tiles = Math.Ceiling(tiles);

            OutputTilesControl.OutputValue = tiles.ToString() + " Tiles";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == SaveButton)
                SaveConcrete();

            else if (sender == SaveButton2)
                SaveSteel();

            else if (sender == SaveButton3)
                SavePaint();

            else if (sender == SaveButton4)
                SaveTiles();
        }
        private void SaveConcrete()
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
                OrderNumber = _data.Orders.Count + 1,
                MaterialName = selectedMaterial.Name,
                Category = selectedMaterial.Category,
                Quantity = Helper.GetNumericalValue(QuantityTxt),
                Unit = selectedMaterial.Unit,
                ElementType = (ElementType)ElementTypeComboBox.SelectedItem,
                UnitPrice = selectedMaterial.UnitPrice,
                Status = "Pending",
                Date = DateTime.Now
            };

            SaveOrder(order);

            MessageBox.Show("Concrete order saved successfully.");
        }
        private void SaveSteel()
        {
            var order = new Order
            {
                OrderNumber = _data.Orders.Count + 1,
                MaterialName = "Steel Bars",
                Category = MaterialType.Steel,
                Quantity = Helper.GetNumericalValue(NoOfBarsTxt),
                Unit = "Bars",
                UnitPrice = 0,
                Status = "Pending",
                Date = DateTime.Now
            };

            SaveOrder(order);

            MessageBox.Show("Steel order saved successfully.");
        }
        private void SavePaint()
        {
            var order = new Order
            {
                OrderNumber = _data.Orders.Count + 1,
                MaterialName = "Paint",
                Category = MaterialType.Paint,
                Quantity = Helper.GetNumericalValue(SurfaceAreaTxt),
                Unit = "m²",
                UnitPrice = 0,
                Status = "Pending",
                Date = DateTime.Now
            };

            SaveOrder(order);

            MessageBox.Show("Paint order saved successfully.");
        }
        private void SaveTiles()
        {
            var order = new Order
            {
                OrderNumber = _data.Orders.Count + 1,
                MaterialName = "Tiles",
                Category = MaterialType.Tiles,
                Quantity = Helper.GetNumericalValue(RoomLengthTxt) * Helper.GetNumericalValue(RoomWidthTxt),
                Unit = "m²",
                UnitPrice = 0,
                Status = "Pending",
                Date = DateTime.Now
            };

            SaveOrder(order);

            MessageBox.Show("Tiles order saved successfully.");
        }
        private void SaveOrder(Order order)
        {
            _data.Orders.Add(order);
            Helper.SaveToJson(_data);
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!IsInitialized) return;

            if (sender is not System.Windows.Controls.TextBox textBox)
                return;

            string input = textBox.Text.Trim();

            bool isEmpty = string.IsNullOrWhiteSpace(input);
            bool isNumber = double.TryParse(input, out double value);

            bool isValid = true;
            string errorMessage = "";

            string[] numericFields =
            {
                "LengthTxt",
                "WidthTxt",
                "DepthTxt",
                "QuantityTxt",
                "BarLengthTxt",
                "NoOfBarsTxt",
                "SurfaceAreaTxt",
                "NoOfCoatsTxt",
                "CoverageRateTxt",
                "RoomLengthTxt",
                "RoomWidthTxt",
                "WasteTxt"
            };

            if (numericFields.Contains(textBox.Name))
            {
                if (isEmpty)
                {
                    isValid = false;
                    errorMessage = "This field cannot be empty";
                }
                else if (!isNumber)
                {
                    isValid = false;
                    errorMessage = "Please enter a valid number";
                }
                else
                {
                    if (textBox.Name == "WasteTxt")
                    {
                        if (value < 0)
                        {
                            isValid = false;
                            errorMessage = "Waste cannot be negative";
                        }
                    }
                    else
                    {
                        if (value <= 0)
                        {
                            isValid = false;
                            errorMessage = "Value must be greater than 0";
                        }
                    }
                }
            }

            if (!isValid)
            {
                textBox.BorderBrush = System.Windows.Media.Brushes.Red;
                textBox.BorderThickness = new Thickness(1.5);
                textBox.ToolTip = errorMessage;
            }
            else
            {
                textBox.ClearValue(BorderBrushProperty);
                textBox.ClearValue(BorderThicknessProperty);
                textBox.ToolTip = null;
            }
        }
    }
}
