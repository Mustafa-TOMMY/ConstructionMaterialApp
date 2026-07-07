using System.Collections.ObjectModel;

namespace ConstructionMaterial.DAL.Models
{
    public class AppData
    {
        public ICollection<MainMaterial> Materials { get; set; } = new List<MainMaterial>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
