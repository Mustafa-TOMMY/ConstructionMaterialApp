using System;
using ConstructionMaterial.BLL.interfaces;

namespace ConstructionMaterial.BLL
{
    public class TilesCalculationService : ITilesCalculationService
    {
        public double CalculateTilesCount(double roomLength, double roomWidth, double tileSize, double wastePercentage)
        {
            if (roomLength <= 0 || roomWidth <= 0 || tileSize <= 0 || wastePercentage < 0)
                return 0;

            double roomArea = roomLength * roomWidth;
            double tilesCount = (roomArea / tileSize) * (1.0 + wastePercentage / 100.0);
            return Math.Ceiling(tilesCount);
        }
    }
}
