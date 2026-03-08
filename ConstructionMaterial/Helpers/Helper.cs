using ConstructionMaterial.Models;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Controls;

namespace ConstructionMaterial.Helpers
{
    public static class Helper
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConstructionMaterialDataBase.json");
        public static void SaveToJson(AppData data)
        {
            string jsonDataFile = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonDataFile);
        }

        public static AppData LoadFromJson()
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

        public static double GetNumericalValue(string textBox)
        {
            double.TryParse(textBox, out double value);
            return value;
        }
    }
}
