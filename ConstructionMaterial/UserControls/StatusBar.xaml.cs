using System.Windows;
using System.Windows.Controls;

namespace ConstructionMaterial.UserControls
{
    /// <summary>
    /// Interaction logic for StatusBar.xaml
    /// </summary>
    public partial class StatusBar : UserControl
    {
        public StatusBar()
        {
            InitializeComponent();
        }

        public void UpdateLastSaved()
        {
            LastSaved = $"Last Saved: {System.DateTime.Now:dd/MM/yyyy HH:mm}";
        }

        public string LastSaved
        {
            get { return (string)GetValue(LastSavedProperty); }
            set { SetValue(LastSavedProperty, value); }
        }

        public static readonly DependencyProperty LastSavedProperty =
            DependencyProperty.Register("LastSaved", typeof(string), 
                typeof(StatusBar), new PropertyMetadata("Last Saved: --"));

        public string MaterialsCount
        {
            get { return (string)GetValue(MaterialsCountProperty); }
            set { SetValue(MaterialsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaterialsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaterialsCountProperty =
            DependencyProperty.Register("MaterialsCount", typeof(string), typeof(StatusBar), new PropertyMetadata("Materials: 0"));
    }
}
