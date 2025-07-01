using System.IO;

namespace BasicTest
{
    public static class LoggerService
    {
        /// <summary>
        /// 获取当天的日志文件路径，格式如 Error-2025-06-29.log
        /// </summary>
        private static string GetLogFilePath()
        {
            string fileName = $"Error-{DateTime.Now:yyyy-MM-dd}.log";
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        /// <summary>
        /// 记录异常信息到日志文件（最新日志在文件开头，按天分文件）
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="source">异常来源（代码位置）</param>
        public static void LogException(Exception exception, string source)
        {
            try
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Source: {source}\n{exception}\n\n";
                string logFilePath = GetLogFilePath();

                // 读取原有内容
                string oldContent = File.Exists(logFilePath) ? File.ReadAllText(logFilePath) : string.Empty;

                // 新日志写在开头
                string newContent = logMessage + oldContent;
                File.WriteAllText(logFilePath, newContent);
            }
            catch
            {
                // 如果日志写入失败，不抛出异常，避免影响主程序运行
            }
        }
    }
}