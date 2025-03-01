using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.BLL
{
    public class EmailServiceConfig
    {

        public static EmailServiceSettings QQMail()
        {
            var mail = new EmailServiceSettings
            {
                Host = "smtp.qq.com",
                Port = 465,
                User = "li.scott@qq.com",
                Password = "ooinbitilcygeaig",
                EnableSsl = true,
                IsHtml = false,
                DisplayName = "邮件通知助手",
                From = "li.scott@qq.com",
            };
            return mail;
        }
        public static EmailServiceSettings _126Mail()
        {
            var mail = new EmailServiceSettings
            {
                Host = "smtp.126.com",
                Port = 465,
                User = "liscott2025@126.com",
                Password = "CPcAdwTZhtHPYLnW",
                EnableSsl = true,
                IsHtml = false,
                DisplayName = "邮件通知助手",
                From = "liscott2025@126.com",
            };
            return mail;
        }
    }
}
