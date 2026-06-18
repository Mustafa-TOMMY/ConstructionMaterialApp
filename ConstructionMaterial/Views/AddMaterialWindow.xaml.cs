using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ConstructionMaterial.DAL.Models.Enum;
using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;

namespace ConstructionMaterial.Views
{
    /// <summary>
    /// Interaction logic for AddMaterialWindow.xaml
    /// </summary>
    public partial class AddMaterialWindow : Window
    {
        public List<MaterialType> MaterialTypes { get; }
        public List<string> MaterialUnits { get; }
        private readonly IMaterialService _materialService;

        public AddMaterialWindow(IMaterialService materialService)
        {
            InitializeComponent();
            _materialService = materialService;
            BtnSave.IsEnabled = false;
            MaterialUnits = new List<string> { "m³", "m²", "kg", "ton", "Liter" };
            MaterialTypes = Enum.GetValues<MaterialType>().ToList();
            DataContext = this;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var material = new MaterialDto()
            {
                Name = MaterialNameTxt.Text,
                Unit = UnitComboBox.Text,
                UnitPrice = double.Parse(UnitPriceTxt.Text),
                Category = ((MaterialType)CategoryBox.SelectedItem).ToString()
            };

            try
            {
                var exists = _materialService.GetAllMaterial().Any(m => m.Name.Equals(MaterialNameTxt.Text, StringComparison.OrdinalIgnoreCase));
                if (!exists)
                {
                    _materialService.AddMaterial(material);
                    MessageBox.Show("Material added successfully!", "Success", MessageBoxButton.OK);
                    Close();
                }
                else
                {
                    MessageBox.Show("Material with the same name already exists!",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding material: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                bool isValid = true;
                string errorMessage = "";

                if (textBox.Name == "UnitPriceTxt")
                {
                    isValid = double.TryParse(textBox.Text, out double num) && num > 0;
                    errorMessage = "Please enter a valid number";
                }
                else
                {
                    bool isNumber = double.TryParse(textBox.Text, out _);
                    if (string.IsNullOrWhiteSpace(textBox.Text) || isNumber)
                    {
                        isValid = false;
                        errorMessage = "Please enter a valid material name";
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

            if (BtnSave != null)
                BtnSave.IsEnabled = isNameValid && isPriceValid;
        }
    }
}
