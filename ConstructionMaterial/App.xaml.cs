using Microsoft.Extensions.DependencyInjection;
using ConstructionMaterial.DAL.Infra;
using ConstructionMaterial.DAL.Infr;
using ConstructionMaterial.BLL;
using ConstructionMaterial.BLL.interfaces;
using ConstructionMaterial.Views;
using ConstructionMaterial.ViewModels;
using System;
using System.Windows;

namespace ConstructionMaterial
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Resolve and show MainWindow
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Repositories
            services.AddSingleton<IMyAppRepo, MyAppRepo>();

            // Services
            services.AddTransient<IMaterialService, MaterialService>();
            services.AddTransient<IOrderService, OrderService>();

            // ViewModels
            services.AddTransient<MaterialViewModel>();

            // Windows
            services.AddTransient<MainWindow>();
            services.AddTransient<CalculatorWindow>();
            services.AddTransient<OrdersWindow>();
            services.AddTransient<AddMaterialWindow>();
        }
    }
}
