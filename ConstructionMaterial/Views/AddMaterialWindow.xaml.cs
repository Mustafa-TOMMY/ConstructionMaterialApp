using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ConstructionMaterial.DAL.Models.Enum;
using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using ConstructionMaterial.ViewModels;

namespace ConstructionMaterial.Views
{
    /// <summary>
    /// Interaction logic for AddMaterialWindow.xaml
    /// </summary>
    public partial class AddMaterialWindow : Wpf.Ui.Controls.FluentWindow
    {
        private readonly MaterialViewModel _materialViewModel;

        public AddMaterialWindow(MaterialViewModel materialViewModel)
        {
            InitializeComponent();
            DataContext = materialViewModel;
            _materialViewModel = materialViewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
