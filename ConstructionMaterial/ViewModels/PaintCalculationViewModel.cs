using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using ConstructionMaterial.Core;
using ConstructionMaterial.DAL.Models.Enum;
using System.Windows;

namespace ConstructionMaterial.ViewModels
{
    public class PaintCalculationViewModel : BaseViewModel
    {
        private readonly IOrderService _orderService;
        private readonly IPaintCalculationService _paintCalculationService;

        private SurfaceType _selectedSurfaceType;
        public SurfaceType SelectedSurfaceType
        {
            get => _selectedSurfaceType;
            set
            {
                _selectedSurfaceType = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private double _surfaceArea;
        public double SurfaceArea
        {
            get => _surfaceArea;
            set
            {
                _surfaceArea = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private double _coats;
        public double Coats
        {
            get => _coats;
            set
            {
                _coats = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private double _coverageRate;
        public double CoverageRate
        {
            get => _coverageRate;
            set
            {
                _coverageRate = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private string _resultText = "0.00 Liters";
        public string ResultText
        {
            get => _resultText;
            set
            {
                _resultText = value;
                OnPropertyChanged();
            }
        }

        public List<SurfaceType> SurfaceTypes { get; }
        public BaseCommand CalculatePaintCommand { get; }
        public BaseCommand SavePaintCommand { get; }

        public PaintCalculationViewModel(IOrderService orderService, IPaintCalculationService paintCalculationService)
        {
            _orderService = orderService;
            _paintCalculationService = paintCalculationService;
            SurfaceTypes = Enum.GetValues(typeof(SurfaceType)).Cast<SurfaceType>().ToList();
            CalculatePaintCommand = new BaseCommand(p => CalculatePaint(), p => IsFormValid());
            SavePaintCommand = new BaseCommand(p => SavePaint(), p => IsFormValid());

            if (SurfaceTypes.Count > 0) SelectedSurfaceType = SurfaceTypes[0];
        }

        private void CalculatePaint()
        {
            if (!IsFormValid())
            {
                ResultText = "0.00 Liters";
                return;
            }
            double liters = _paintCalculationService.CalculateLiters(SurfaceArea, Coats, CoverageRate);
            ResultText = $"{liters:0.00} Liters";
        }

        private void SavePaint()
        {
            if (!IsFormValid())
            {
                MessageBox.Show("Please enter valid surface area, coats and coverage rate first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var nextOrderNumber = _orderService.GetAllOrders().Count + 1;
            var order = new OrderDto
            {
                OrderNumber = nextOrderNumber,
                MaterialName = $"Paint ({SelectedSurfaceType})",
                Category = MaterialType.Paint.ToString(),
                Quantity = SurfaceArea,
                Unit = "m²",
                UnitPrice = 0,
                Total = 0,
                ElementType = ElementType.Wall.ToString(),
                Status = "Pending",
                Date = DateTime.Now
            };

            _orderService.AddOrder(order);
            MessageBox.Show("Paint order saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool IsFormValid()
        {
            return SurfaceArea > 0 && Coats > 0 && CoverageRate > 0;
        }

        private void RaiseCanExecuteChanged()
        {
            CalculatePaintCommand.RaiseCanExecuteChanged();
            SavePaintCommand.RaiseCanExecuteChanged();
        }
    }
}
