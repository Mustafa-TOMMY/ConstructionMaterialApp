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
    /// Interaction logic for PaintCalculationTab.xaml
    /// </summary>
    public partial class PaintCalculationTab : UserControl
    {
        public PaintCalculationTab()
        {
            InitializeComponent();
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

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
