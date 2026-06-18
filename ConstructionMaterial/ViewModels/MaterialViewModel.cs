using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using ConstructionMaterial.Core;
using ConstructionMaterial.DAL.Models.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace ConstructionMaterial.ViewModels
{
    public class MaterialViewModel : BaseViewModel
    {

        #region Properities
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
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

        private string _unit;
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
            }
        } 
        #endregion

        public ICommand GetMaterialsCommand;
        public ICommand AddMaterialCommand;

        public List<MaterialType> MaterialTypes { get; }
        public List<string> MaterialUnits { get; }

        public ObservableCollection<MaterialDto> MaterialCatalog { get; set; } = new ObservableCollection<MaterialDto>();
        public IMaterialService _materialService { get; }
        public MaterialViewModel(IMaterialService materialService)
        {
            MaterialUnits = new List<string> { "m³", "m²", "kg", "ton", "Liter" };
            MaterialTypes = Enum.GetValues<MaterialType>().ToList();
            Category = MaterialTypes.First();
            Unit = MaterialUnits.First();


            _materialService = materialService;
            GetMaterialsCommand = new BaseCommand(p => GetMaterials(), p => true);
            AddMaterialCommand = new BaseCommand(p => AddMaterial(), p => true);
            GetMaterials();
        }
        public void GetMaterials()
        {
            var materials = _materialService.GetAllMaterial();

            MaterialCatalog = new ObservableCollection<MaterialDto>(materials);

            OnPropertyChanged(nameof(MaterialCatalog));
        }
        public void AddMaterial()
        {
            var material = new MaterialDto
            {
                Name = Name,
                Category = Category,
                Unit = Unit,
                UnitPrice = UnitPrice
            };

            _materialService.AddMaterial(material);
            GetMaterials();
        }
    }
}
