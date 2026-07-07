using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using ConstructionMaterial.Core;
using ConstructionMaterial.DAL.Models.Enum;
using System.Collections.ObjectModel;
using System.Windows;

namespace ConstructionMaterial.ViewModels
{
    public class MaterialViewModel : ValidationBaseViewModel
    {
        public IMaterialService _materialService { get; }

        #region Apply INotifyChanged on fields
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                AddMaterialCommand.RaiseCanExecuteChanged();
                if(string.IsNullOrWhiteSpace(Name))
                {
                    AddError("Name is required.");
                }
                else if (!IsMaterialNameDublicated())
                {
                    AddError("Material name already exist.");
                }
                else
                {
                    ClearError();
                }
            }
        }

        private MaterialType _category;
        public MaterialType Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        private string _unit = string.Empty;
        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                OnPropertyChanged();
            }
        }

        private double _unitPrice;
        public double UnitPrice
        {
            get => _unitPrice;
            set
            {
                _unitPrice = value;
                OnPropertyChanged();
                AddMaterialCommand.RaiseCanExecuteChanged();
                if (UnitPrice <= 0)
                {
                    AddError("Price must be a positive number.");
                }
                else
                {
                    ClearError();
                }
            }
        }

        private int _totalNumberOfMaterial;
        public int TotalNumberOfMaterial
        {
            get => _totalNumberOfMaterial;
            set
            {
                _totalNumberOfMaterial = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MaterialDto> _materialCatalog = new();
        public ObservableCollection<MaterialDto> MaterialCatalog
        {
            get => _materialCatalog;
            set
            {
                _materialCatalog = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public BaseCommand GetMaterialsCommand { get; }
        public BaseCommand AddMaterialCommand { get; }
        public BaseCommand RemoveMaterialCommand { get; }
        public BaseCommand UpdateMaterialCommand { get; }

        #endregion

        public event Action? OpenMaterialWindowRequested;
        public event Action? CloseRequested;
        public bool IsEditMode => _editingMaterialId.HasValue;


        private int? _editingMaterialId;
        #region DefaultValues
        public List<MaterialType> MaterialTypes { get; }
        public List<string> MaterialUnits { get; }
        #endregion

        public MaterialViewModel(IMaterialService materialService)
        {
            MaterialUnits = new List<string> { "m³", "m²", "kg", "ton", "Liter" };
            MaterialTypes = Enum.GetValues<MaterialType>().ToList();
            Category = MaterialTypes.First();
            Unit = MaterialUnits.First();

            _materialService = materialService;
            GetMaterialsCommand = new BaseCommand(p => GetMaterials(), p => true);
            AddMaterialCommand = new BaseCommand(p => SaveMaterial(), p => IsMaterialFormValid());
            RemoveMaterialCommand = new BaseCommand(p => DeleteMaterial(p as MaterialDto), p => p is MaterialDto);
            UpdateMaterialCommand = new BaseCommand(p => StartEditMaterial(p as MaterialDto), p => p is MaterialDto);
            GetMaterials();
        }
        public void GetMaterials()
        {
            var materials = _materialService.GetAllMaterial();
            MaterialCatalog = new ObservableCollection<MaterialDto>(materials);
        }
        public void SaveMaterial()
        {
            var material = new MaterialDto
            {
                Id = _editingMaterialId ?? 0,
                Name = Name,
                Category = Category,
                Unit = Unit,
                UnitPrice = UnitPrice
            };

            if (IsEditMode)
                _materialService.UpdateMaterial(material);
            else
                _materialService.AddMaterial(material);

            ClearForm();
            GetMaterials();

            CloseRequested?.Invoke();
        }
        public void StartEditMaterial(MaterialDto? material)
        {
            if (material == null)
                return;

            _editingMaterialId = material.Id;

            Name = material.Name;
            Category = material.Category;
            Unit = material.Unit;
            UnitPrice = material.UnitPrice;

            OpenMaterialWindowRequested?.Invoke();
        }
        public void DeleteMaterial(MaterialDto? material)
        {
            if (material == null) return;
            var result = MessageBox.Show(
                $"Are you sure you want to delete {material.Name}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _materialService.RemoveMaterial(material.Id);
                GetMaterials();
            }
        }
        public bool IsMaterialFormValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && IsMaterialNameDublicated() && UnitPrice > 0;
        }
        public bool IsMaterialNameDublicated()
        {
            return !_materialService.GetAllMaterial().Any(m => m.Name.Equals(Name, StringComparison.OrdinalIgnoreCase) && m.Id != _editingMaterialId);
        }
        public void ClearForm()
        {
            _editingMaterialId = null;

            Name = string.Empty;
            Category = MaterialTypes.First();
            Unit = MaterialUnits.First();
            UnitPrice = 0;
        }
    }
}

