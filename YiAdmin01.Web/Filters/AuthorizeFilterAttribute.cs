using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YiAdmin01.BLL.Business;
using YiAdmin01.Common.Extension;
using YiAdmin01.Common.Utils;
using YiAdmin01.Model;
using YiAdmin01.Model.Result;
using YiAdmin01.WebCode;

namespace YiAdmin01.Web.Filters
{
    /// <summary>
    /// 权限过滤
    /// </summary>
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///  权限字符串，例如 organization:user:view
        /// </summary>
        public string Authorize { get; set; }

        #region 构造函数
        public AuthorizeFilterAttribute() { }

        public AuthorizeFilterAttribute(string authorize)
        {
            this.Authorize = authorize;
        }


        #endregion

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool hasPermission = false;

            OperatorInfo user = await Operator.Instance.Current();
            //登陆判断
            if (user == null || user.UserId == 0)
            {//未登录
                // 防止用户选择记住我，页面一直在首页刷新
                if (CookieHelper.Get("RememberMe").ParseToInt() == 1)
                {
                    Operator.Instance.RemoveCurrent();
                }

                if (context.HttpContext.Request.IsAjaxRequest() )
                {
                    TData obj = new TData();
                    obj.Message = "抱歉，没有登录或登录已超时";
                    context.Result = new JsonResult(obj);
                    return;
                }
                else
                {   // Redirect to Login
                    context.Result = new RedirectResult("~/Home/Login");
                    return;
                }
            }
            else
            {
                // admin
                if (user.IsSystem == 1)
                {
                    hasPermission = true;
                }
                else
                {
                    // 权限判断
                    if (!string.IsNullOrEmpty(Authorize))
                    {
                        string[] authorizeList = Authorize.Split(',');
                        TData<List<MenuAuthorizeInfo>> objMenuAuthorize = await new MenuAuthorizeBLL().GetAuthorizeList(user);
                        List<MenuAuthorizeInfo> menuAuthorizeInfoList = objMenuAuthorize.Data.Where(p => authorizeList.Contains(p.Authorize)).ToList();

                        if (menuAuthorizeInfoList.Count > 0)
                        {
                            hasPermission = true;

                            #region 新增和修改 权限判断
                            if (context.RouteData.Values["Action"].ToString() == "SaveFormJson")
                            {
                                var id = context.HttpContext.Request.Form["Id"];
                                if (id.ParseToLong() > 0)
                                {
                                    if (!menuAuthorizeInfoList.Where(p => p.Authorize.Contains("edit")).Any())
                                    {
                                        hasPermission = false;
                                    }
                                }
                                else
                                {
                                    if (!menuAuthorizeInfoList.Where(p => p.Authorize.Contains("add")).Any())
                                    {
                                        hasPermission = false;
                                    }
                                }
                            }
                            #endregion
                        }
                        if (!hasPermission)
                        {
                            if (context.HttpContext.Request.IsAjaxRequest())
                            {
                                TData obj = new TData();
                                obj.Message = "抱歉，没有权限";
                                context.Result = new JsonResult(obj);
                            }
                            else
                            {
                                context.Result = new RedirectResult("~/Home/NoPermission");
                            }
                        }
                    }
                    else
                    {
                        hasPermission = true;
                    }
                }
                if (hasPermission)
                {
                    var resultContext = await next();
                }
            }

        }
    }
}
