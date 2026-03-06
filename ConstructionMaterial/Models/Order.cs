using ConstructionMaterial.Models.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConstructionMaterial.Models
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public string MaterialName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public MaterialType Category { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ElementType ElementType { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
        public double Total => Quantity * UnitPrice; 
        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; } // Pending or Delivered
    }
}
