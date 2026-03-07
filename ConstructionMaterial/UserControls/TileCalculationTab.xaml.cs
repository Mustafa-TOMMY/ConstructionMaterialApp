using ConstructionMaterial.Helpers;
using ConstructionMaterial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConstructionMaterial.UserControls
{
    /// <summary>
    /// Interaction logic for TileCalculationTab.xaml
    /// </summary>
    public partial class TileCalculationTab : UserControl
    {
        public TileCalculationTab()
        {
            InitializeComponent();
        }
        public string OutputTilesValue
        {
            get { return (string)GetValue(OutputTilesValueProperty); }
            set { SetValue(OutputTilesValueProperty, value); }
        }

        public static readonly DependencyProperty OutputTilesValueProperty =
            DependencyProperty.Register("OutputTilesValue", typeof(string), typeof(TileCalculationTab), new PropertyMetadata("0 Tiles"));

        private void TilesTabCalculateButton_Click(object sender, RoutedEventArgs e)
        {
            double length = Helper.GetNumericalValue(RoomLengthTxt);
            double width = Helper.GetNumericalValue(RoomWidthTxt);
            double waste = Helper.GetNumericalValue(WasteTxt);

            if (TileSizeComboBox.SelectedItem is Tile selectedTile)
            {
                double roomArea = length * width;
                double tileArea = selectedTile.Size; // تأكد أن الموديل Tile يحتوي على Size

                if (tileArea > 0)
                {
                    double totalTiles = (roomArea / tileArea) * (1 + waste / 100);
                    OutputTilesValue = Math.Ceiling(totalTiles).ToString() + " Tiles";
                }
            }
            else
            {
                MessageBox.Show("Please select a tile size.");
            }
        }

        private void TilesTabSaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tiles Order logic goes here.");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) { }
    }
}
