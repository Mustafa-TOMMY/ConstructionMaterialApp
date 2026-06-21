using ConstructionMaterial.BLL.interfaces;

namespace ConstructionMaterial.BLL
{
    public class SteelCalculationService : ISteelCalculationService
    {
        public (double TotalWeight, double Tons) CalculateWeight(double diameter, double length, double barsCount)
        {
            if (diameter <= 0 || length <= 0 || barsCount <= 0)
                return (0, 0);

            double weightPerBar = (diameter * diameter / 162.0) * length;
            double totalWeight = weightPerBar * barsCount;
            double tons = totalWeight / 1000.0;
            return (totalWeight, tons);
        }
    }
}
