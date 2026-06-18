using ConstructionMaterial.DAL.Models.Enum;

namespace ConstructionMaterial.BLL.DTOs
{
    public class MaterialDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MaterialType Category { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
    }
}
