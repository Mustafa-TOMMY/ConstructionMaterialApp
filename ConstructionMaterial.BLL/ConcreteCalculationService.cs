using ConstructionMaterial.BLL.interfaces;

namespace ConstructionMaterial.BLL
{
    public class ConcreteCalculationService : IConcreteCalculationService
    {
        public double CalculateVolume(double length, double width, double depth, int quantity)
        {
            if (length <= 0 || width <= 0 || depth <= 0 || quantity <= 0)
                return 0;
            return length * width * depth * quantity * 1.1; // 10% waste factor
        }
    }
}
