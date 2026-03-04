using System.Collections.ObjectModel;

namespace ConstructionMaterial.Models
{
    public class AppData
    {
        public ObservableCollection<MainMaterial> Materials { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
    }
}
