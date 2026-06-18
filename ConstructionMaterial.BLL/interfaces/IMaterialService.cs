using ConstructionMaterial.BLL.DTOs;
using System;
using System.Collections.Generic;

namespace ConstructionMaterial.BLL.interfaces
{
    public interface IMaterialService
    {
        /// <summary>
        /// Retrieves all stored materials mapped into a list of Data Transfer Objects (DTOs).
        /// </summary>
        /// <returns>A list of <see cref="MaterialDto"/> representing all available materials.</returns>
        List<MaterialDto> GetAllMaterial();

        /// <summary>
        /// Retrieves a specific material by its unique identifier.
        /// </summary>
        /// <param name="materiaId">The unique identifier of the material.</param>
        /// <returns>A <see cref="MaterialDto"/> containing the matching material's data.</returns>
        /// <exception cref="Exception">Thrown when no material matches the provided identifier.</exception>
        MaterialDto GetMaterialById(int materiaId);

        /// <summary>
        /// Adds a new material to the data store by mapping a Data Transfer Object (DTO) to the core domain model.
        /// </summary>
        /// <param name="material">The material data transfer object containing the details to add.</param>
        void AddMaterial(MaterialDto material);

        /// <summary>
        /// Updates the attributes of an existing material based on the provided Data Transfer Object.
        /// </summary>
        /// <param name="material">The material data transfer object containing updated values and a valid ID.</param>
        /// <exception cref="Exception">Thrown when no material matches the provided DTO's identifier.</exception>
        void UpdateMaterial(MaterialDto material);

        /// <summary>
        /// Removes a material from the data store based on its unique identifier.
        /// </summary>
        /// <param name="materiaId">The unique identifier of the material to be removed.</param>
        /// <exception cref="Exception">Thrown when no material matches the provided identifier.</exception>
        void RemoveMaterial(int materiaId);
    }
}