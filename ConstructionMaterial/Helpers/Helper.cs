using System.Windows.Controls;

namespace ConstructionMaterial.Helpers
{
    public static class Helper
    {
        public static double GetNumericalValue(TextBox textBox)
        {
            double.TryParse(textBox.Text, out double value);
            return value;
        }
    }
}
