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
using System.Windows.Shapes;

namespace ConstructionMaterial.Views
{
    /// <summary>
    /// Interaction logic for UpdateMaterialWindow.xaml
    /// </summary>
    public partial class UpdateMaterialWindow : Window
    {
        public List<MaterialType> MaterialTypes { get; }
        public List<string> MaterialUnits { get; }
        private AppData _data;
        private MainMaterial _materialToUpdate;

        public UpdateMaterialWindow(AppData data, MainMaterial selectedMaterial)
        {
            InitializeComponent();
            _data = data;
            _materialToUpdate = selectedMaterial;

            MaterialUnits = new List<string> { "m³", "m²", "kg", "ton", "Liter" };
            MaterialTypes = Enum.GetValues<MaterialType>().ToList();

            DataContext = this;
            PopulateFields();
        }

        private void PopulateFields()
        {
            CategoryBox.SelectedItem = _materialToUpdate.Category;
            MaterialNameTxt.Text = _materialToUpdate.Name;
            UnitComboBox.SelectedItem = _materialToUpdate.Unit;
            UnitPriceTxt.Text = _materialToUpdate.UnitPrice.ToString();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

            _materialToUpdate.Name = MaterialNameTxt.Text;
            _materialToUpdate.Unit = UnitComboBox.Text;
            _materialToUpdate.UnitPrice = double.Parse(UnitPriceTxt.Text);
            _materialToUpdate.Category = (MaterialType)CategoryBox.SelectedItem;

            Helper.SaveToJson(_data);
            MessageBox.Show("Material updated successfully!", "Success", MessageBoxButton.OK);
            DialogResult = true;
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsInitialized) return;

            if (sender is TextBox textBox)
            {
                bool isValid = true;
                string errorMessage = "";

                if (textBox.Name == "UnitPriceTxt")
                {
                    isValid = double.TryParse(textBox.Text, out double num) && num > 0;
                    errorMessage = "Please enter a valid price";
                }
                else
                {
                    bool isNumber = double.TryParse(textBox.Text, out _);
                    if (string.IsNullOrWhiteSpace(textBox.Text) || isNumber)
                    {
                        isValid = false;
                        errorMessage = "Please enter a valid name";
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
            bool isPriceValid = double.TryParse(UnitPriceTxt.Text, out double price) && price > 0;

            if (BtnUpdate != null)
                BtnUpdate.IsEnabled = isNameValid && isPriceValid;
        }
    }
}
