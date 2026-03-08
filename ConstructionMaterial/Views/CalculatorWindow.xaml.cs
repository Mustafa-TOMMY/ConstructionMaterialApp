using ConstructionMaterial.Models;
using ConstructionMaterial.Models.Enum;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace ConstructionMaterial.Views
{
    /// <summary>
    /// Interaction logic for CalculatorWindow.xaml
    /// </summary>
    public partial class CalculatorWindow : Window, INotifyPropertyChanged
    {
        public List<ElementType> ElementTypes { get; set; }
        public List<BarDiameter> BarDiameters { get; set; }
        public List<MainMaterial> MaterialNames { get; set; }
        public List<MainMaterial> SteelMaterials { get; set; }
        public List<MainMaterial> TileMaterials { get; set; }
        public List<MainMaterial> PaintMaterials { get; set; }
        public List<SurfaceType> SurfaceTypes { get; set; }
        public List<Tile> TileSizes { get; set; }
        public ObservableCollection<Order> Orders { get; set; }


        #region INotify properity

        private string _steelOutputValue;
        public string SteelOutputValue
        {
            get => _steelOutputValue;
            set
            {
                _steelOutputValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SteelOutputValue)));
            }
        }


        private string _concreteOutputValue;
        public string ConcreteOutputValue
        {
            get => _concreteOutputValue;
            set
            {
                _concreteOutputValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteOutputValue)));
            }
        }


        private string _paintOutputValue;
        public string PaintOutputValue
        {
            get => _paintOutputValue;
            set
            {
                _paintOutputValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaintOutputValue)));
            }
        }


        private string _tilesOutputValue;
        public string TilesOutputValue
        {
            get => _tilesOutputValue;
            set
            {
                _tilesOutputValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TilesOutputValue)));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion



        public MainWindow Main { get; set; }

        public CalculatorWindow(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            ElementTypes = Enum.GetValues(typeof(ElementType)).Cast<ElementType>().ToList();
            BarDiameters = Enum.GetValues(typeof(BarDiameter)).Cast<BarDiameter>().ToList();
            SurfaceTypes = Enum.GetValues(typeof(SurfaceType)).Cast<SurfaceType>().ToList();
            MaterialNames = Main.MaterialCatalog.Where(m => m.Category == MaterialType.Concrete).ToList();
            SteelMaterials = Main.MaterialCatalog.Where(m => m.Category == MaterialType.Steel).ToList();
            TileMaterials = Main.MaterialCatalog.Where(m => m.Category == MaterialType.Tiles).ToList();
            PaintMaterials = Main.MaterialCatalog.Where(m => m.Category == MaterialType.Paint).ToList();
            TileSizes = new List<Tile>
            {
                new Tile { Name = "30x30", Size = 0.09 },
                new Tile { Name = "50x50", Size = 0.1 },
                new Tile { Name = "60x60", Size = 0.12 },
                new Tile { Name = "80x80", Size = 0.64 }
            };
            DataContext = this;
        }
        private void AddNewOrder(Order obj)
        {
            obj.OrderNumber = Main.Orders.Count + 1;
            Main.Orders.Add(obj);
            Main.SaveData();
        }
    }
}
