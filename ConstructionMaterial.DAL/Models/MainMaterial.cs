using ConstructionMaterial.DAL.Models.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConstructionMaterial.DAL.Models
{
    public class MainMaterial
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonConverter(typeof(StringEnumConverter))]
        public MaterialType Category { get; set; }
        public string Unit { get; set; } = string.Empty;
        public double UnitPrice { get; set; }
    }
}
