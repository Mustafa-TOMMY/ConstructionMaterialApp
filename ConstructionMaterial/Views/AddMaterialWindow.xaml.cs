using System.Windows;
using ConstructionMaterial.ViewModels;

namespace ConstructionMaterial.Views
{
    /// <summary>
    /// Interaction logic for AddMaterialWindow.xaml
    /// </summary>
    public partial class AddMaterialWindow : Window
    {
        private readonly MaterialViewModel _materialViewModel;

        public AddMaterialWindow(MaterialViewModel materialViewModel)
        {
            InitializeComponent();
            DataContext = materialViewModel;
            _materialViewModel = materialViewModel;

            System.Action closeAction = () => Close();
            _materialViewModel.CloseRequested += closeAction;
            this.Closed += (s, e) => _materialViewModel.CloseRequested -= closeAction;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
