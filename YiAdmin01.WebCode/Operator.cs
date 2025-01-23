using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using YiAdmin01.Common.Cache;
using YiAdmin01.Common.Global;
using YiAdmin01.Common.Utils;

namespace YiAdmin01.WebCode
{
    public class Operator
    {
        public static Operator Instance
        {
            get { return new Operator(); }
        }

        private string LoginProvider = GlobalContext.Configuration.GetSection("SystemConfig:LoginProvider").Value;
        private string TokenName = "UserToken"; //cookie name or session name

        /// <summary>
        /// 为当前用户添加Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task AddCurrent(string token)
        {

            #region 
            switch (LoginProvider)
            {
                case "Cookie":
                    CookieHelper.Set(TokenName, token);
                    break;

                case "Session":
                    SessionHelper.Set(TokenName, token);
                    break;

                case "WebApi":
                    OperatorInfo user = await new DataRepository().GetUserByToken(token);
                    if (user != null)
                    {
                        CacheFactory.Cache.SetCache(token, user);
                    }
                    break;

                default:
                    throw new Exception("未找到LoginProvider配置");
            }
            #endregion
        }

        /// <summary>
        /// Api接口需要传入apiToken
        /// </summary>
        /// <param name="apiToken"></param>
        public void RemoveCurrent(string apiToken = "")
        {

            #region
            switch (LoginProvider)
            {
                case "Cookie":
                    CookieHelper.Remove(TokenName);
                    break;

                case "Session":
                    SessionHelper.Remove(TokenName);
                    break;

                case "WebApi":
                    CacheFactory.Cache.RemoveCache(apiToken);
                    break;

                default:
                    throw new Exception("未找到LoginProvider配置");
            }
            #endregion
        }

        /// <summary>
        /// Api接口需要传入apiToken
        /// </summary>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public async Task<OperatorInfo> Current(string apiToken = "")
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            OperatorInfo user = null;
            string token = string.Empty;

            switch (LoginProvider)
            {
                case "Cookie":
                    if (hca.HttpContext != null)
                    {
                        token = CookieHelper.Get(TokenName);
                    }
                    break;

                case "Session":
                    if (hca.HttpContext != null)
                    {
                        token = SessionHelper.Get(TokenName);
                    }
                    break;

                case "WebApi":
                    token = apiToken;
                    break;
            }

           
            if (string.IsNullOrEmpty(token))
            {
                return user;
            }

            token = token.Trim('"');
            user = CacheFactory.Cache.GetCache<OperatorInfo>(token);
            if (user == null)
            {
                user = await new DataRepository().GetUserByToken(token);
                if (user != null)
                {
                    CacheFactory.Cache.SetCache(token, user);
                }
            }
            return user;
        }
    }
}
