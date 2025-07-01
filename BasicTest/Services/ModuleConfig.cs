using System.IO;
using System.Text.Json;
using System.Windows;

namespace BasicTest
{
    /// <summary>
    /// 模块配置管理类
    /// </summary>
    public static class ModuleConfig
    {
        private const string CONFIG_FILE = "modules.json";
        public static string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIG_FILE);

        /// <summary>
        /// 加载模块配置
        /// </summary>
        /// <returns>模块信息列表</returns>
        public static List<ModuleInfo> LoadConfig()
        {
            try
            {
                if (!File.Exists(ConfigFilePath))
                {
                    throw new FileNotFoundException("配置文件不存在", ConfigFilePath);
                }
                string json = File.ReadAllText(ConfigFilePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<List<ModuleInfo>>(json, options) ?? [];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载模块配置失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return [];
        }

        /// <summary>
        /// 保存模块配置
        /// </summary>
        /// <param name="modules">模块信息列表</param>
        public static void SaveConfig(List<ModuleInfo> modules)
        {
            try
            {
                if (!File.Exists(ConfigFilePath))
                {
                    throw new FileNotFoundException("配置文件不存在", ConfigFilePath);
                }
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(modules, options);
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存模块配置失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}