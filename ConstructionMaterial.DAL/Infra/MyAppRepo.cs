using ConstructionMaterial.DAL.Infra;
using ConstructionMaterial.DAL.Models;
using Newtonsoft.Json;

namespace ConstructionMaterial.DAL.Infra 
{
    public  class MyAppRepo : IMyAppRepo
    {
        private string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConstructionMaterialDataBase.json");
        public void SaveToJson(AppData data)
        {
            string jsonDataFile = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonDataFile);
        }

        public AppData LoadFromJson()
        {
            if (!File.Exists(filePath))
                return new AppData();
            try
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<AppData>(jsonData) ?? new AppData();
            }
            catch (JsonException)
            {
                return new AppData();
            }
        }
    }
}
