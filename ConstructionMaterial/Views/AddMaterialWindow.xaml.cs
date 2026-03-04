using ConstructionMaterial.Helpers;
using ConstructionMaterial.Models;
using ConstructionMaterial.Models.Enum;
using System.Windows;

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

            MaterialTypes = Enum.GetValues<MaterialType>().ToList();
            DataContext = this;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MainMaterial material = new MainMaterial()
            {
                Name = MaterialNameTxt.Text,
                Unit = UnitTxt.Text,
                UnitPrice = Helper.GetNumericalValue(UnitPriceTxt),
                Category = (MaterialType)CategoryBox.SelectedItem
            };
            _data.Materials.Add(material);
            Helper.SaveToJson(_data);
            MessageBox.Show("Material added successfully!", "Success", MessageBoxButton.OK);
        }
    }
}
