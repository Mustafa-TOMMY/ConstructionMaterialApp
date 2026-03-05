using ConstructionMaterial.Models.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConstructionMaterial.Models
{
    public class MainMaterial
    {
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public MaterialType Category { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
    }
}
