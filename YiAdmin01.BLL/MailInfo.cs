using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace YiAdmin01.BLL
{
    /// <summary>
    /// 发送邮件的信息
    /// </summary>
    public class MailInfo
    {

        /// <summary>
        /// 接收者名字
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 接收者邮箱（多个用英文","号分割）
        /// </summary>
        public string Receivers{ get; set; }

        /// <summary>
        /// 邮件标题
        /// </summary>
        public string Subject
        {
            get;set;
        }

        /// <summary>
        /// 正文内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 抄送人集合（多个用英文","分割）
        /// </summary>
        public string CC { get; set; }

        /// <summary>
        /// 回复地址
        /// </summary>
        public string Reply { get; set; }
    }
}
