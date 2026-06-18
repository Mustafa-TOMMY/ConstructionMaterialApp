using System;

namespace ConstructionMaterial.BLL.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public string MaterialName { get; set; }
        public string Category { get; set; }
        public string ElementType { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
