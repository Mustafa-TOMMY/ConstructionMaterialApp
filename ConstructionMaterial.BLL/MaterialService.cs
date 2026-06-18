using ConstructionMaterial.DAL.Infra;
using ConstructionMaterial.DAL.Models;
using ConstructionMaterial.DAL.Models.Enum;
using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionMaterial.BLL
{
    public class MaterialService : IMaterialService
    {
        private readonly IMyAppRepo _myAppRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialService"/> class.
        /// </summary>
        /// <param name="myAppRepo">The repository layer dependency used to load and save data.</param>
        public MaterialService(IMyAppRepo myAppRepo)
        {
            _myAppRepo = myAppRepo;
        }

        public void AddMaterial(MaterialDto material)
        {
            var data = _myAppRepo.LoadFromJson();
            var materialMapper = new MainMaterial()
            {
                Name = material.Name,
                Category = material.Category,
                Unit = material.Unit,
                UnitPrice = material.UnitPrice
            };
            data.Materials.Add(materialMapper);
            _myAppRepo.SaveToJson(data);
        }

        public List<MaterialDto> GetAllMaterial()
        {
            var data = _myAppRepo.LoadFromJson();
            var material = data.Materials;
            return material.Select(m => new MaterialDto
            {
                Id = m.Id,
                Name = m.Name,
                Category = m.Category,
                Unit = m.Unit,
                UnitPrice = m.UnitPrice
            }).ToList();
        }

        public MaterialDto GetMaterialById(int materiaId)
        {
            var data = _myAppRepo.LoadFromJson();
            var material = data.Materials.FirstOrDefault(m => m.Id == materiaId);
            if (material != null)
            {
                var materialMapper = new MaterialDto()
                {
                    Id = material.Id,
                    Name = material.Name,
                    Category = material.Category,
                    Unit = material.Unit,
                    UnitPrice = material.UnitPrice
                };
                return materialMapper;
            }
            else
            {
                throw new Exception($"Material with name '{materiaId}' not found.");
            }
        }

        public void RemoveMaterial(int materiaId)
        {
            var data = _myAppRepo.LoadFromJson();
            var materialToRemove = data.Materials.FirstOrDefault(m => m.Id == materiaId);
            if (materialToRemove == null)
            {
                throw new Exception($"Material with name '{materiaId}' not found.");
            }
            data.Materials.Remove(materialToRemove);
            _myAppRepo.SaveToJson(data);
        }

        public void UpdateMaterial(MaterialDto material)
        {
            var data = _myAppRepo.LoadFromJson();
            var existingMaterial = data.Materials.FirstOrDefault(m => m.Id == material.Id);
            if (existingMaterial == null)
            {
                throw new Exception($"Material with ID '{material.Id}' not found.");
            }

            existingMaterial.Name = material.Name;
            existingMaterial.Category =  material.Category;
            existingMaterial.Unit = material.Unit;
            existingMaterial.UnitPrice = material.UnitPrice;
            _myAppRepo.SaveToJson(data);
        }
    }
}