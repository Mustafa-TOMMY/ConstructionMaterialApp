using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using ConstructionMaterial.Core;
using ConstructionMaterial.DAL.Models;
using ConstructionMaterial.DAL.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ConstructionMaterial.ViewModels
{
    public class TilesCalculationViewModel : BaseViewModel
    {
        private readonly IMaterialService _materialService;
        private readonly IOrderService _orderService;
        private readonly ITilesCalculationService _tilesCalculationService;

        private double _roomLength;
        public double RoomLength
        {
            get => _roomLength;
            set
            {
                _roomLength = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private double _roomWidth;
        public double RoomWidth
        {
            get => _roomWidth;
            set
            {
                _roomWidth = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private Tile? _selectedTile;
        public Tile? SelectedTile
        {
            get => _selectedTile;
            set
            {
                _selectedTile = value;
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

        private double _wastePercentage = 10.0;
        public double WastePercentage
        {
            get => _wastePercentage;
            set
            {
                _wastePercentage = value;
                OnPropertyChanged();
                RaiseCanExecuteChanged();
            }
        }

        private string _resultText = "0 Tiles";
        public string ResultText
        {
            get => _resultText;
            set
            {
                _resultText = value;
                OnPropertyChanged();
            }
        }

        public List<Tile> TileSizes { get; }
        public List<MaterialDto> MaterialNames { get; }
        public BaseCommand CalculateTilesCommand { get; }
        public BaseCommand SaveTilesCommand { get; }

        public TilesCalculationViewModel(IMaterialService materialService, IOrderService orderService, ITilesCalculationService tilesCalculationService)
        {
            _materialService = materialService;
            _orderService = orderService;
            _tilesCalculationService = tilesCalculationService;

            TileSizes = new List<Tile>
            {
                new Tile { Name = "30x30", Size = 0.09 },
                new Tile { Name = "50x50", Size = 0.1 },
                new Tile { Name = "60x60", Size = 0.12 },
                new Tile { Name = "80x80", Size = 0.64 }
            };

            CalculateTilesCommand = new BaseCommand(p => CalculateTiles(), p => IsFormValid());
            SaveTilesCommand = new BaseCommand(p => SaveTiles(), p => IsFormValid());

            MaterialNames = _materialService.GetAllMaterial()
                .Where(m => m.Category == MaterialType.Tiles)
                .ToList();

            if (TileSizes.Count > 1) SelectedTile = TileSizes[1]; // Default to 50x50
            if (MaterialNames.Count > 0) SelectedMaterial = MaterialNames[0];
        }

        private void CalculateTiles()
        {
            if (!IsFormValid() || SelectedTile == null)
            {
                ResultText = "0 Tiles";
                return;
            }
            double tilesCount = _tilesCalculationService.CalculateTilesCount(RoomLength, RoomWidth, SelectedTile.Size, WastePercentage);

            ResultText = $"{tilesCount} Tiles";
        }

        private void SaveTiles()
        {
            if (!IsFormValid() || SelectedTile == null || SelectedMaterial == null)
            {
                MessageBox.Show("Please enter valid room dimensions, waste percentage, select a tile size, and select a material first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double tilesCount = _tilesCalculationService.CalculateTilesCount(RoomLength, RoomWidth, SelectedTile.Size, WastePercentage);
            double qty = RoomLength * RoomWidth;

            var nextOrderNumber = _orderService.GetAllOrders().Count + 1;
            var order = new OrderDto
            {
                OrderNumber = nextOrderNumber,
                MaterialName = SelectedMaterial.Name,
                Category = MaterialType.Tiles.ToString(),
                Quantity = qty,
                Unit = "m2",
                UnitPrice = SelectedMaterial.UnitPrice,
                Total = qty * SelectedMaterial.UnitPrice,
                ElementType = ElementType.Floor.ToString(),
                Status = "Pending",
                Date = DateTime.Now
            };

            _orderService.AddOrder(order);
            MessageBox.Show("Tiles order saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool IsFormValid()
        {
            return RoomLength > 0 && RoomWidth > 0 && WastePercentage >= 0 && SelectedTile != null && SelectedMaterial != null;
        }

        private void RaiseCanExecuteChanged()
        {
            CalculateTilesCommand.RaiseCanExecuteChanged();
            SaveTilesCommand.RaiseCanExecuteChanged();
        }
    }
}
