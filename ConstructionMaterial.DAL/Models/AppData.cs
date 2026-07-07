using System.Collections.ObjectModel;

namespace ConstructionMaterial.DAL.Models
{
    public class AppData
    {
        public ICollection<MainMaterial> Materials { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
