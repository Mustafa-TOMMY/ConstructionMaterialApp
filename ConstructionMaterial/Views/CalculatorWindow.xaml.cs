using ConstructionMaterial.ViewModels;
using System.Windows;

namespace ConstructionMaterial.Views
{
    /// <summary>
    /// Interaction logic for CalculatorWindow.xaml
    /// </summary>
    public partial class CalculatorWindow : Window
    {
        public CalculatorWindow(MainCalculatorViewModel mainCalculatorViewModel)
        {
            InitializeComponent();
            
            DataContext = mainCalculatorViewModel;

            // Wire up the DataContexts of each UserControl tab to their sub-ViewModels
            ConcreteCalculatorControl.DataContext = mainCalculatorViewModel.ConcreteCalculationViewModel;
            SteelCalculatorControl.DataContext = mainCalculatorViewModel.SteelCalculationViewModel;
            PaintCalculatorControl.DataContext = mainCalculatorViewModel.PaintCalculationViewModel;
            TilesCalculatorControl.DataContext = mainCalculatorViewModel.TilesCalculationViewModel;
        }
    }
}
