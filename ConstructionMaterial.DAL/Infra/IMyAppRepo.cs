using ConstructionMaterial.DAL.Models;

namespace ConstructionMaterial.DAL.Infra
{
    public interface IMyAppRepo
    {
        /// <summary>
        /// Save the data to json file, the file will be created in the same directory
        /// as the application if it does not exist, if it exists it will be overwritten
        /// </summary>
        /// <param name="data"></param>
        public void SaveToJson(AppData data);
        /// <summary>
        /// Load the data from json file, if the file does not exist it will return an empty AppData object
        /// </summary>
        /// <returns></returns>
        public AppData LoadFromJson();

    }
}
