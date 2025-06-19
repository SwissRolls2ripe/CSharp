using System.Windows;

namespace MvvmLightDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModels.StudentVM vm = new ViewModels.StudentVM();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = vm;
            studentDisplayView.SetViewModel(vm);
        }
    }
}