using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using ConstructionMaterial.Core;
using ConstructionMaterial.DAL.Models.Enum;
using System.Windows;

namespace ConstructionMaterial.ViewModels
{
    public class ConcreteCalculationViewModel : BaseViewModel
    {
        private readonly IMaterialService _materialService;
        private readonly IOrderService _orderService;
        private readonly IConcreteCalculationService _concreteCalculationService;

        private double _length;
        public double Length
        {
            get => _length;
            set
            {
                _length = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private double _width;
        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private double _depth;
        public double Depth
        {
            get => _depth;
            set
            {
                _depth = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private ElementType _selectedElementType;
        public ElementType SelectedElementType
        {
            get => _selectedElementType;
            set
            {
                _selectedElementType = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private MaterialDto? _selectedMaterial;
        public MaterialDto? SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                _selectedMaterial = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private string _resultText = "0.00 m3";
        public string ResultText
        {
            get => _resultText;
            set
            {
                _resultText = value;
                OnPropertyChanged();
            }
        }

        public BaseCommand CalculateConcreteCommand { get; }
        public BaseCommand SaveConcreteCommand { get; }

        public List<ElementType> ElementTypes { get; }
        public List<MaterialDto> MaterialNames { get; }

        public ConcreteCalculationViewModel(IMaterialService materialService, IOrderService orderService, IConcreteCalculationService concreteCalculationService)
        {
            _materialService = materialService;
            _orderService = orderService;
            _concreteCalculationService = concreteCalculationService;

            MaterialNames = _materialService.GetAllMaterial()
                .Where(m => m.Category == MaterialType.Concrete)
                .ToList();

            ElementTypes = Enum.GetValues(typeof(ElementType))
                .Cast<ElementType>()
                .ToList();

            CalculateConcreteCommand = new BaseCommand(p => CalculateConcrete(), p => IsFormValid());
            SaveConcreteCommand = new BaseCommand(p => SaveConcrete(), p => IsFormValid());

            // Initialize default values
            if (ElementTypes.Count > 0) SelectedElementType = ElementTypes[0];
            if (MaterialNames.Count > 0) SelectedMaterial = MaterialNames[0];
            Quantity = 1; // Default quantity
        }

        private void CalculateConcrete()
        {
            if (!IsFormValid())
            {
                ResultText = "0.00 m3";
                return;
            }
            double volume = _concreteCalculationService.CalculateVolume(Length, Width, Depth, Quantity);
            ResultText = $"{volume:0.00} m3";
        }

        private void SaveConcrete()
        {
            if (!IsFormValid() || SelectedMaterial == null)
            {
                MessageBox.Show("Please enter valid positive dimensions, quantity and select a material first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double volume = _concreteCalculationService.CalculateVolume(Length, Width, Depth, Quantity);

            var nextOrderNumber = _orderService.GetAllOrders().Count + 1;
            var order = new OrderDto
            {
                OrderNumber = nextOrderNumber,
                MaterialName = SelectedMaterial.Name,
                Category = MaterialType.Concrete.ToString(),
                Quantity = volume,
                Unit = "m3",
                UnitPrice = SelectedMaterial.UnitPrice,
                Total = volume * SelectedMaterial.UnitPrice,
                ElementType = SelectedElementType.ToString(),
                Status = "Pending",
                Date = DateTime.Now
            };

            _orderService.AddOrder(order);
            MessageBox.Show("Concrete order saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool IsFormValid()
        {
            return Length > 0 && Width > 0 && Depth > 0 && Quantity > 0 && SelectedMaterial != null;
        }

        private void RaiseCanExecuteChanged()
        {
            CalculateConcreteCommand.RaiseCanExecuteChanged();
            SaveConcreteCommand.RaiseCanExecuteChanged();
        }
    }
}
