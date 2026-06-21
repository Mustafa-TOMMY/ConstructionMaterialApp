namespace ConstructionMaterial.BLL.interfaces
{
    public interface ISteelCalculationService
    {
        (double TotalWeight, double Tons) CalculateWeight(double diameter, double length, double barsCount);
    }
}
