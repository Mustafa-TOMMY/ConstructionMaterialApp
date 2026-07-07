using ConstructionMaterial.BLL.interfaces;

namespace ConstructionMaterial.BLL
{
    public class PaintCalculationService : IPaintCalculationService
    {
        public double CalculateLiters(double surfaceArea, double coats, double coverageRate)
        {
            if (surfaceArea <= 0 || coats <= 0 || coverageRate <= 0)
                return 0;
            return (surfaceArea * coats) / coverageRate;
        }
    }
}
