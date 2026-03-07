using ConstructionMaterial.Helpers;
using ConstructionMaterial.Models;
using ConstructionMaterial.Models.Enum;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConstructionMaterial.Views
{
    /// <summary>
    /// Interaction logic for AddMaterialWindow.xaml
    /// </summary>
    public partial class AddMaterialWindow : Window
    {
        public List<MaterialType> MaterialTypes { get; }
        private AppData _data { get; set; }
        public AddMaterialWindow(AppData data)
        {
            InitializeComponent();
            _data = data;
            BtnSave.IsEnabled = false;
            MaterialTypes = Enum.GetValues<MaterialType>().ToList();
            DataContext = this;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MainMaterial material = new MainMaterial()
            {
                Name = MaterialNameTxt.Text,
                Unit = UnitTxt.Text,
                UnitPrice = double.Parse(UnitPriceTxt.Text),
                Category = (MaterialType)CategoryBox.SelectedItem
            };
            _data.Materials.Add(material);
            Helper.SaveToJson(_data);
            MessageBox.Show("Material added successfully!", "Success", MessageBoxButton.OK);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                bool isValid = true;
                string errorMessage = "";

                if (textBox.Name == "UnitPriceTxt")
                {
                    isValid = double.TryParse(textBox.Text, out _);
                    errorMessage = "Please enter a valid number";
                }
                else
                {
                    bool isNumber = double.TryParse(textBox.Text, out _);
                    if (string.IsNullOrWhiteSpace(textBox.Text) || isNumber)
                    {
                        isValid = false;
                        errorMessage = "This field cannot be empty";
                    }
                }


                if (!isValid && !string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.BorderBrush = Brushes.Red;
                    textBox.BorderThickness = new Thickness(1.5);
                    textBox.ToolTip = errorMessage;
                }
                else
                {
                    textBox.ClearValue(BorderBrushProperty);
                    textBox.ToolTip = null;
                }
            }
            TurnOnOffBtn();
        }

        private void TurnOnOffBtn()
        {
            bool isNameValid = !string.IsNullOrWhiteSpace(MaterialNameTxt.Text) && !double.TryParse(MaterialNameTxt.Text, out _);
            bool isUnitValid = !string.IsNullOrWhiteSpace(UnitTxt.Text) && !double.TryParse(UnitTxt.Text, out _);
            bool isPriceValid = double.TryParse(UnitPriceTxt.Text, out double price) && price > 0;

            if (BtnSave != null)
                BtnSave.IsEnabled = isNameValid && isUnitValid && isPriceValid;
        }
        
    }
}

