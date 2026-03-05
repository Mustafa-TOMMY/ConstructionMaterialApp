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
    /// Interaction logic for CalculationOutput.xaml
    /// </summary>
    public partial class CalculationOutput : UserControl
    {
        public CalculationOutput()
        {
            InitializeComponent();
        }



        public string OutputLabel
        {
            get { return (string)GetValue(OutputLabelProperty); }
            set { SetValue(OutputLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutputLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutputLabelProperty =
            DependencyProperty.Register("OutputLabel", typeof(string), typeof(CalculationOutput), new PropertyMetadata("0"));





        public string OutputValue
        {
            get { return (string)GetValue(OutputValueProperty); }
            set { SetValue(OutputValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutputValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutputValueProperty =
            DependencyProperty.Register("OutputValue", typeof(string), typeof(CalculationOutput), new PropertyMetadata("0"));



    }
}
