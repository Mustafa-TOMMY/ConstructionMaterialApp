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
        public BaseCommand CalculateTilesCommand { get; }
        public BaseCommand SaveTilesCommand { get; }

        public TilesCalculationViewModel(IOrderService orderService, ITilesCalculationService tilesCalculationService)
        {
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

            if (TileSizes.Count > 1) SelectedTile = TileSizes[1]; // Default to 50x50
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
            if (!IsFormValid() || SelectedTile == null)
            {
                MessageBox.Show("Please enter valid room dimensions, waste percentage and select a tile size first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var nextOrderNumber = _orderService.GetAllOrders().Count + 1;
            var order = new OrderDto
            {
                OrderNumber = nextOrderNumber,
                MaterialName = $"Tiles ({SelectedTile.Name})",
                Category = MaterialType.Tiles.ToString(),
                Quantity = RoomLength * RoomWidth,
                Unit = "m²",
                UnitPrice = 0,
                Total = 0,
                ElementType = ElementType.Floor.ToString(),
                Status = "Pending",
                Date = DateTime.Now
            };

            _orderService.AddOrder(order);
            MessageBox.Show("Tiles order saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool IsFormValid()
        {
            return RoomLength > 0 && RoomWidth > 0 && WastePercentage >= 0 && SelectedTile != null;
        }

        private void RaiseCanExecuteChanged()
        {
            CalculateTilesCommand.RaiseCanExecuteChanged();
            SaveTilesCommand.RaiseCanExecuteChanged();
        }
    }
}
