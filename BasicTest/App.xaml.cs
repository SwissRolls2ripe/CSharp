using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace BasicTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            // 注册服务和ViewModel
            services.AddSingleton<IAudioService, AudioService>();
            services.AddTransient<DoubleColorBallViewModel>();

            ServiceProvider = services.BuildServiceProvider();

            base.OnStartup(e);
        }
    }
}
