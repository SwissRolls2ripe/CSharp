using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BasicTest
{
    public class MailService
    {
        private readonly string smtpHost = "smtp.qq.com"; // SMTP服务器地址
        private readonly int smtpPort = 587; // 推荐使用587端口
        private readonly bool enableSsl = true; // 启用SSL加密
        private readonly string mailSenderAddress;
        private readonly string password;

        public MailService(string mailSenderAddress, string password)
        {
            this.mailSenderAddress = mailSenderAddress;
            this.password = password;
        }

        public void SendMails(string mailSenderAddress, string mailSenderName, string subject, string address, string filePath, string bodyContent)
        {
            MailMessage mm = null;
            Attachment att = null;
            try
            {
                // 创建邮件消息
                mm = new MailMessage
                {
                    From = new MailAddress(mailSenderAddress, mailSenderName, Encoding.UTF8),
                    Subject = subject,
                    BodyEncoding = Encoding.UTF8,
                    Body = bodyContent
                };

                // 添加收件人
                mm.To.Add(new MailAddress(address));

                // 添加附件（如果存在）
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    att = new Attachment(filePath);
                    mm.Attachments.Add(att);
                }

                // 发送邮件
                SendMail(mm);
            }
            finally
            {
                // 确保资源释放
                att?.Dispose();
                mm?.Dispose();
            }
        }

        private void SendMail(MailMessage ms)
        {
            try
            {
                // 每次发送邮件时创建新的 SmtpClient 实例
                using (var smtpClient = new SmtpClient
                {
                    Host = smtpHost,
                    Port = smtpPort,
                    EnableSsl = enableSsl,
                    Credentials = new NetworkCredential(mailSenderAddress, password)
                })
                {
                    smtpClient.Send(ms);
                }
            }
            catch (SmtpException smtpEx)
            {
                // 记录详细的SMTP异常信息到日志
                LoggerService.LogException(smtpEx, "MailServer.SendMail");
                throw;
            }
            catch (Exception ex)
            {
                // 捕获其他异常并记录到日志
                LoggerService.LogException(ex, "MailServer.SendMail");
                throw;
            }
        }
    }
}
