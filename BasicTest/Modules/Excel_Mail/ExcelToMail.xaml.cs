using System.Data;
using System.Windows;

namespace BasicTest
{
    /// <summary>
    /// ExcelToMail.xaml 的交互逻辑
    /// </summary>
    public partial class ExcelToMail : Window
    {
        public ExcelToMail()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择Excel文件
        /// </summary>
        private void SelectExcel_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xlsm;*.xls|All Files|*.*",
                Title = "选择Excel文件"
            };
            if (ofd.ShowDialog() == true)
            {
                ExcelPath.Text = ofd.FileName;
            }
        }

        private void SendMail_Click(object sender, RoutedEventArgs e)
        {
            var dt = ExcelService.ReadExcelToDataTable(ExcelPath.Text);
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Excel文件读取失败或没有数据。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Dictionary<string, string> mailSendContent = new();
            foreach (DataRow row in dt.Rows)
            {
                // 假设邮件地址在第一列
                var mailAddress = row[0].ToString();
                var attachment = row[1].ToString();

                if (!string.IsNullOrEmpty(mailAddress))
                {
                    mailSendContent.Add(mailAddress, attachment);
                }
            }

            if (mailSendContent.Count > 0)
            {
                var mailServer = new MailService(MailSenderAddress.Text, MailPass.Text);
                foreach (var kvp in mailSendContent)
                {
                    try
                    {
                        mailServer.SendMails(MailSenderAddress.Text, MailSenderName.Text, MailSubject.Text, kvp.Key, kvp.Value, MailContent.Text);
                    }
                    catch (Exception ex)
                    {
                        // 记录异常到日志
                        LoggerService.LogException(ex, $"ExcelToMail.SendMail_Click - Sending to {kvp.Key}");
                    }
                }
            }
            MessageBox.Show("邮件发送完成！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
