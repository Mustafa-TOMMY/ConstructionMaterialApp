using ConstructionMaterial.Helpers;
using ConstructionMaterial.Models;
using ConstructionMaterial.Models.Enum;
using System.Windows;
using System.Windows.Controls;

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
        }

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

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}