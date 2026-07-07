using System;

namespace ConstructionMaterial.BLL.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ElementType { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public double UnitPrice { get; set; }
        public double Total { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
