using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.Model
{
    /// <summary>
    /// 这个是移动端Api用的
    /// </summary>
    public class BaseApiToken
    {
        [NotMapped]
        [Description("WebApi没有Cookie和Session，所以需要传入Token来标识用户身份，请加在Url后面")]
        public string Token { get; set; }
    }
}
