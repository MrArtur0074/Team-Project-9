using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using Oswalt.Services;
using Oswalt.ViewModels;
using Oswalt.Views;
using Project9.Services;
using Project9.ViewModels;
using System;
using ControlzEx.Theming;

namespace Oswalt
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;
        public static DialogCoordinator DialogCoordinator => (DialogCoordinator)DialogCoordinator.Instance;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ConfigureServices();

            ThemeManager.Current.ChangeTheme(this, "Dark.Blue");

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Регистрация сервисов
            services.AddSingleton<UndoRedoService>();
            services.AddSingleton<DwgExportService>();
            services.AddSingleton<RibsInterpolationService>();
            services.AddSingleton<IDialogCoordinator>(_ => DialogCoordinator.Instance);

            // ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<WingGeometryViewModel>();
            // ...

            // Окна
            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            ServiceProvider = services.BuildServiceProvider();
        }
    }

}