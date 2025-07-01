using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BasicTest.UserControls
{
    /// <summary>
    /// ExcelFileSelect.xaml 的交互逻辑
    /// </summary>
    public partial class ExcelFileSelect : UserControl, INotifyPropertyChanged
    {
        private string? _excelPath;

        public string? ExcelFilePath
        {
            get => _excelPath;
            set
            {
                if (_excelPath != value)
                {
                    _excelPath = value;
                    OnPropertyChanged(nameof(ExcelFilePath));
                }
            }
        }
        public ExcelFileSelect()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SelectExcel_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xlsm;*.xls|All Files|*.*",
                Title = "选择Excel文件"
            };
            if (ofd.ShowDialog() == true)
            {
                ExcelFilePath = ofd.FileName;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
