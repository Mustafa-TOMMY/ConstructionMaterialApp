using ConstructionMaterial.Core;
using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConstructionMaterial.ViewModels
{
    public class MaterialViewModel : BaseViewModel
    {
        public ICommand getMaterialsCommand;
        public ICommand addMaterialCommand;
        public ObservableCollection<MaterialDto> MaterialCatalog { get; set; } = new ObservableCollection<MaterialDto>();
        public IMaterialService _materialService { get; }
        public MaterialViewModel(IMaterialService materialService)
        {
            _materialService = materialService;
            getMaterialsCommand = new BaseCommand(p => GetMaterials(), p => true);
            addMaterialCommand = new BaseCommand(p => AddMaterial(), p => true);
            GetMaterials();
        }
        public void GetMaterials()
        {
            var materials = _materialService.GetAllMaterial();

            MaterialCatalog = new ObservableCollection<MaterialDto>(materials);

            OnPropertyChanged(nameof(MaterialCatalog));
        }
        public void AddMaterial(MaterialDto material)
        {
            _materialService.AddMaterial(material);
        }
    }
}
