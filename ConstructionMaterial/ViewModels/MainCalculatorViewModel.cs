using ConstructionMaterial.Core;

namespace ConstructionMaterial.ViewModels
{
    public class MainCalculatorViewModel : BaseViewModel
    {
        public MainCalculatorViewModel(
            ConcreteCalculationViewModel concreteCalculationViewModel,
            SteelCalculationViewModel steelCalculationViewModel,
            PaintCalculationViewModel paintCalculationViewModel,
            TilesCalculationViewModel tilesCalculationViewModel)
        {
            ConcreteCalculationViewModel = concreteCalculationViewModel;
            SteelCalculationViewModel = steelCalculationViewModel;
            PaintCalculationViewModel = paintCalculationViewModel;
            TilesCalculationViewModel = tilesCalculationViewModel;
        }

        public ConcreteCalculationViewModel ConcreteCalculationViewModel { get; }
        public SteelCalculationViewModel SteelCalculationViewModel { get; }
        public PaintCalculationViewModel PaintCalculationViewModel { get; }
        public TilesCalculationViewModel TilesCalculationViewModel { get; }
    }
}
