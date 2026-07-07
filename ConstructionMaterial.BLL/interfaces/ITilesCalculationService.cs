namespace ConstructionMaterial.BLL.interfaces
{
    public interface ITilesCalculationService
    {
        double CalculateTilesCount(double roomLength, double roomWidth, double tileSize, double wastePercentage);
    }
}
