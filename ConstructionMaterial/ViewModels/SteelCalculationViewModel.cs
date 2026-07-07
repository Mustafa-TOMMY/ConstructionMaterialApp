using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using ConstructionMaterial.Core;
using ConstructionMaterial.DAL.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ConstructionMaterial.ViewModels
{
    public class SteelCalculationViewModel : BaseViewModel
    {
        private readonly IMaterialService _materialService;
        private readonly IOrderService _orderService;
        private readonly ISteelCalculationService _steelCalculationService;

        private BarDiameter _selectedDiameter;
        public BarDiameter SelectedDiameter
        {
            get => _selectedDiameter;
            set
            {
                _selectedDiameter = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

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

        private double _barsCount;
        public double BarsCount
        {
            get => _barsCount;
            set
            {
                _barsCount = value;
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

        private string _resultText = "0.00 kg | 0.000 ton";
        public string ResultText
        {
            get => _resultText;
            set
            {
                _resultText = value;
                OnPropertyChanged();
            }
        }

        public List<BarDiameter> BarDiameters { get; }
        public List<MaterialDto> MaterialNames { get; }
        public BaseCommand CalculateSteelCommand { get; }
        public BaseCommand SaveSteelCommand { get; }

        public SteelCalculationViewModel(IMaterialService materialService, IOrderService orderService, ISteelCalculationService steelCalculationService)
        {
            _materialService = materialService;
            _orderService = orderService;
            _steelCalculationService = steelCalculationService;

            BarDiameters = Enum.GetValues(typeof(BarDiameter)).Cast<BarDiameter>().ToList();
            CalculateSteelCommand = new BaseCommand(p => CalculateSteel(), p => IsFormValid());
            SaveSteelCommand = new BaseCommand(p => SaveSteel(), p => IsFormValid());

            MaterialNames = _materialService.GetAllMaterial()
                .Where(m => m.Category == MaterialType.Steel)
                .ToList();

            if (BarDiameters.Count > 0) SelectedDiameter = BarDiameters[0];
            if (MaterialNames.Count > 0) SelectedMaterial = MaterialNames[0];
        }

        private void CalculateSteel()
        {
            if (!IsFormValid())
            {
                ResultText = "0.00 kg | 0.000 ton";
                return;
            }
            double diameter = (double)SelectedDiameter;
            var (totalWeight, tons) = _steelCalculationService.CalculateWeight(diameter, Length, BarsCount);

            ResultText = $"{totalWeight:0.00} kg | {tons:0.000} ton";
        }

        private void SaveSteel()
        {
            if (!IsFormValid() || SelectedMaterial == null)
            {
                MessageBox.Show("Please enter a valid length, count and select a material first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double diameter = (double)SelectedDiameter;
            var (totalWeight, tons) = _steelCalculationService.CalculateWeight(diameter, Length, BarsCount);

            double qty = BarsCount;
            string unit = "Bars";
            double price = SelectedMaterial.UnitPrice;

            if (SelectedMaterial.Unit.Equals("ton", StringComparison.OrdinalIgnoreCase))
            {
                qty = tons;
                unit = "ton";
            }
            else if (SelectedMaterial.Unit.Equals("kg", StringComparison.OrdinalIgnoreCase))
            {
                qty = totalWeight;
                unit = "kg";
            }

            var nextOrderNumber = _orderService.GetAllOrders().Count + 1;
            var order = new OrderDto
            {
                OrderNumber = nextOrderNumber,
                MaterialName = SelectedMaterial.Name,
                Category = MaterialType.Steel.ToString(),
                Quantity = qty,
                Unit = unit,
                UnitPrice = price,
                Total = qty * price,
                ElementType = ElementType.Column.ToString(),
                Status = "Pending",
                Date = DateTime.Now
            };

            _orderService.AddOrder(order);
            MessageBox.Show("Steel order saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool IsFormValid()
        {
            return Length > 0 && BarsCount > 0 && SelectedMaterial != null;
        }

        private void RaiseCanExecuteChanged()
        {
            CalculateSteelCommand.RaiseCanExecuteChanged();
            SaveSteelCommand.RaiseCanExecuteChanged();
        }
    }
}
