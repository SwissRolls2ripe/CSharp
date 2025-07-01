using System.IO;

namespace BasicTest
{
    public static class LoggerService
    {
        /// <summary>
        /// ��ȡ�������־�ļ�·������ʽ�� Error-2025-06-29.log
        /// </summary>
        private static string GetLogFilePath()
        {
            string fileName = $"Error-{DateTime.Now:yyyy-MM-dd}.log";
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        /// <summary>
        /// ��¼�쳣��Ϣ����־�ļ���������־���ļ���ͷ��������ļ���
        /// </summary>
        /// <param name="exception">�쳣����</param>
        /// <param name="source">�쳣��Դ������λ�ã�</param>
        public static void LogException(Exception exception, string source)
        {
            try
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Source: {source}\n{exception}\n\n";
                string logFilePath = GetLogFilePath();

                // ��ȡԭ������
                string oldContent = File.Exists(logFilePath) ? File.ReadAllText(logFilePath) : string.Empty;

                // ����־д�ڿ�ͷ
                string newContent = logMessage + oldContent;
                File.WriteAllText(logFilePath, newContent);
            }
            catch
            {
                // �����־д��ʧ�ܣ����׳��쳣������Ӱ������������
            }
        }
    }
}