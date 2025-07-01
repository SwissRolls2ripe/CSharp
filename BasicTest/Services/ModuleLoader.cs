using System.Reflection;
using System.Windows;

namespace BasicTest
{
    /// <summary>
    /// 模块加载器类
    /// </summary>
    public class ModuleLoader
    {
        private Dictionary<string, ModuleInfo> _moduleCache;

        public ModuleLoader()
        {
            _moduleCache = new Dictionary<string, ModuleInfo>();
        }

        /// <summary>
        /// 加载所有模块信息
        /// </summary>
        /// <returns>模块信息列表</returns>
        public List<ModuleInfo> LoadModules()
        {
            var modules = ModuleConfig.LoadConfig();
            // 更新缓存
            _moduleCache.Clear();
            foreach (var module in modules)
            {
                _moduleCache[module.Id] = module;
            }

            return modules;
        }

        /// <summary>
        /// 创建模块实例
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns>模块窗口实例</returns>
        public Window? CreateModuleInstance(string moduleId)
        {
            try
            {
                if (string.IsNullOrEmpty(moduleId))
                {
                    throw new ArgumentNullException(nameof(moduleId), "模块ID不能为空");
                }

                if (_moduleCache.TryGetValue(moduleId, out ModuleInfo? moduleInfo))
                {
                    if (moduleInfo == null)
                    {
                        throw new InvalidOperationException($"模块信息为空: {moduleId}");
                    }

                    Assembly assembly = string.IsNullOrEmpty(moduleInfo.AssemblyName) ?
                        Assembly.GetExecutingAssembly() :
                        Assembly.Load(moduleInfo.AssemblyName);

                    Type? type = assembly.GetType(moduleInfo.TypeName);
                    if (type == null)
                    {
                        throw new TypeLoadException($"找不到类型: {moduleInfo.TypeName}");
                    }

                    object? instance = Activator.CreateInstance(type);
                    if (instance is Window window)
                    {
                        return window;
                    }
                    else
                    {
                        throw new InvalidCastException($"类型 {moduleInfo.TypeName} 不是Window类型");
                    }
                }
                else
                {
                    throw new KeyNotFoundException($"找不到模块: {moduleId}");
                }
            }
            catch (Exception ex)
            {
                // 记录异常到日志
                LoggerService.LogException(ex, $"ModuleLoader.CreateModuleInstance - ModuleId: {moduleId}");
                MessageBox.Show($"创建模块实例失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}