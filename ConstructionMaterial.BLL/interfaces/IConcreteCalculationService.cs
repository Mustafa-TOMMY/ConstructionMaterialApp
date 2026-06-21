namespace ConstructionMaterial.BLL.interfaces
{
    public interface IConcreteCalculationService
    {
        double CalculateVolume(double length, double width, double depth, int quantity);
    }
}
