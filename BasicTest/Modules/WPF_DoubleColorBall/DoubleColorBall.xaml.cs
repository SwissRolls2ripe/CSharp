using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace BasicTest
{
    /// <summary>
    /// DoubleColorBall.xaml 的交互逻辑
    /// </summary>
    public partial class DoubleColorBall : Window
    {
        public DoubleColorBall()
        {
            InitializeComponent();
            // 通过DI获取ViewModel
            this.DataContext = App.ServiceProvider?.GetRequiredService<DoubleColorBallViewModel>();
        }
    }
}
