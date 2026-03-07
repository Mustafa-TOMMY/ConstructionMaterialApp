using ConstructionMaterial.Helpers;
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
        public SteelCalculationTab()
        {
            InitializeComponent();
        }



        public string OutputSteelValue
        {
            get { return (string)GetValue(OutputSteelValueProperty); }
            set { SetValue(OutputSteelValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutputSteelValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutputSteelValueProperty =
            DependencyProperty.Register("OutputSteelValue", typeof(string), typeof(SteelCalculationTab), new PropertyMetadata(""));



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SteelTabSaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SteelTabCalculateButton_Click(object sender, RoutedEventArgs e)
        {
            double diameter = Convert.ToDouble(BarDiameterComboBox.SelectedItem);
            double length = Helper.GetNumericalValue(BarLengthTxt);
            double bars = Helper.GetNumericalValue(NoOfBarsTxt);
            double weightPerBar = (diameter * diameter / 162) * length;
            double totalWeight = weightPerBar * bars;
            double tons = totalWeight / 1000;

            OutputSteelValue = totalWeight.ToString("0.00") + " kg | " + tons.ToString("0.000") + " ton";
        }
    }
}
