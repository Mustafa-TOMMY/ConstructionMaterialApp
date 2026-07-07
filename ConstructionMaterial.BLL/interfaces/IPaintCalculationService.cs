namespace ConstructionMaterial.BLL.interfaces
{
    public interface IPaintCalculationService
    {
        double CalculateLiters(double surfaceArea, double coats, double coverageRate);
    }
}
