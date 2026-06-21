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
        public BaseCommand CalculateSteelCommand { get; }
        public BaseCommand SaveSteelCommand { get; }

        public SteelCalculationViewModel(IOrderService orderService, ISteelCalculationService steelCalculationService)
        {
            _orderService = orderService;
            _steelCalculationService = steelCalculationService;
            BarDiameters = Enum.GetValues(typeof(BarDiameter)).Cast<BarDiameter>().ToList();
            CalculateSteelCommand = new BaseCommand(p => CalculateSteel(), p => IsFormValid());
            SaveSteelCommand = new BaseCommand(p => SaveSteel(), p => IsFormValid());

            if (BarDiameters.Count > 0) SelectedDiameter = BarDiameters[0];
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
            if (!IsFormValid())
            {
                MessageBox.Show("Please enter a valid length and count first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var nextOrderNumber = _orderService.GetAllOrders().Count + 1;
            var order = new OrderDto
            {
                OrderNumber = nextOrderNumber,
                MaterialName = $"Steel Bars ({SelectedDiameter})",
                Category = MaterialType.Steel.ToString(),
                Quantity = BarsCount,
                Unit = "Bars",
                UnitPrice = 0,
                Total = 0,
                ElementType = ElementType.Column.ToString(),
                Status = "Pending",
                Date = DateTime.Now
            };

            _orderService.AddOrder(order);
            MessageBox.Show("Steel order saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool IsFormValid()
        {
            return Length > 0 && BarsCount > 0;
        }

        private void RaiseCanExecuteChanged()
        {
            CalculateSteelCommand.RaiseCanExecuteChanged();
            SaveSteelCommand.RaiseCanExecuteChanged();
        }
    }
}
