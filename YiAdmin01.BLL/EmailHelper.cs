using MailKit.Net.Smtp;
using MailKit;
using MimeKit;



namespace YiAdmin01.BLL
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    public class EmailHelper
    {
        private MimeMessage mailMessage;
        public SmtpClient smtpClient;

        /// <summary>
        /// 邮件服务设置
        /// </summary>
        public EmailServiceSettings emailServiceSettings { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settings"></param>
        public EmailHelper(EmailServiceSettings settings, MailInfo mailInfo)
        {
            mailMessage = new MimeMessage();
            try
            {
                emailServiceSettings = settings;

                mailMessage.From.Add(new MailboxAddress(settings.DisplayName, settings.From));
                mailMessage.To.Add(new MailboxAddress(mailInfo.ReceiverName, mailInfo.Receivers));
                mailMessage.Subject = mailInfo.Subject;
                mailMessage.Body = new TextPart("plain")
                {
                    Text = mailInfo.Body
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SendMail()
        {
            try
            {
                if(mailMessage == null) { return false; }

                smtpClient = new SmtpClient();
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtpClient.Connect(emailServiceSettings.Host, emailServiceSettings.Port, true);
                smtpClient.Authenticate(emailServiceSettings.User, emailServiceSettings.Password);

                await smtpClient.SendAsync(mailMessage);                
               
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


    }
}

