using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace BasicTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ModuleLoader _moduleLoader;
        private ObservableCollection<ModuleListItem> _modules;
        private List<ModuleListItem> _allModules;

        public MainWindow()
        {
            InitializeComponent();
            _moduleLoader = new ModuleLoader();
            LoadModules();
        }

        /// <summary>
        /// 加载所有模块信息
        /// </summary>
        private void LoadModules()
        {
            // 加载模块配置
            var moduleInfos = _moduleLoader.LoadModules();
            if (moduleInfos == null || moduleInfos.Count == 0)
            {
                MessageBox.Show($"无法加载模块，请重启！", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 创建列表项
            _allModules = new List<ModuleListItem>();
            int index = 1;

            foreach (var module in moduleInfos)
            {
                if (module.IsEnabled)
                {
                    _allModules.Add(new ModuleListItem
                    {
                        Index = index++,
                        Id = module.Id,
                        DisplayName = module.DisplayName
                    });
                }
            }

            // 显示到列表中
            _modules = new ObservableCollection<ModuleListItem>(_allModules);
            ModuleListView.ItemsSource = _modules;
        }

        /// <summary>
        /// 搜索按钮点击事件
        /// </summary>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchBox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // 如果搜索框为空，显示所有模块
                _modules.Clear();
                foreach (var module in _allModules)
                {
                    _modules.Add(module);
                }
            }
            else
            {
                // 根据搜索文本筛选模块
                var filteredModules = _allModules.Where(m =>
                    m.DisplayName.ToLower().Contains(searchText) ||
                    m.Index.ToString().Contains(searchText)).ToList();

                _modules.Clear();
                foreach (var module in filteredModules)
                {
                    _modules.Add(module);
                }
            }
        }

        /// <summary>
        /// 打开模块按钮点击事件
        /// </summary>
        private void OpenModule_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string moduleId)
            {
                // 打开对应的模块窗口
                Window moduleWindow = _moduleLoader.CreateModuleInstance(moduleId);
                if (moduleWindow != null)
                {
                    moduleWindow.Show();
                }
                else
                {
                    MessageBox.Show($"无法加载模块: {moduleId}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    /// <summary>
    /// 模块列表项类
    /// </summary>
    public class ModuleListItem
    {
        public int Index { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }
}