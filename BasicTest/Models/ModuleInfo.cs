namespace BasicTest
{
    /// <summary>
    /// 模块信息类
    /// </summary>
    public class ModuleInfo
    {
        /// <summary>
        /// 模块唯一标识-
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// 模块显示名称
        /// </summary>
        public required string DisplayName { get; set; }

        /// <summary>
        /// 模块窗体类型的完全限定名
        /// </summary>
        public required string TypeName { get; set; }

        /// <summary>
        /// 包含模块的程序集名称
        /// </summary>
        public required string AssemblyName { get; set; }

        /// <summary>
        /// 模块是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}