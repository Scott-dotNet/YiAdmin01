using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using YiAdmin01.BLL.Business;
using YiAdmin01.Common.Extension;
using YiAdmin01.Common.Global;
using YiAdmin01.Common.Utils;
using YiAdmin01.DAL.Enum;
using YiAdmin01.Model;
using YiAdmin01.Entity;
using YiAdmin01.Model.Result;
using YiAdmin01.WebCode;
using YiAdmin01.Web.Filters;

namespace YiAdmin01.Web.Controllers
{
    public class HomeController : Controller
    {
        private UserBLL userBLL {  get; set; }
        private MenuBLL menuBLL { get; set; }
        private MenuAuthorizeBLL menuAuthorizeBLL { get; set; }

        #region 构造函数
        public HomeController()
        {
            this.userBLL = new UserBLL();
            this.menuBLL = new MenuBLL();
            this.menuAuthorizeBLL = new MenuAuthorizeBLL();
        }
        #endregion

        #region Views

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter]
        public async Task<IActionResult> Index()
        {
            var operatorInfo = await Operator.Instance.Current();

            var ObjMenu = await menuBLL.GetList();
            List<MenuEntity> menuList = ObjMenu.Data;
            // 过滤出已启用的菜单列表
            menuList = menuList.Where(p => p.MenuStatus == StatusEnum.Yes.ParseToInt()).ToList();
     

            if (operatorInfo.IsSystem != 1)
            {
                TData<List<MenuAuthorizeInfo>> objMenuAuthorize = await menuAuthorizeBLL.GetAuthorizeList(operatorInfo);
                //过滤出用户有权限的菜单ID列表
                var authorizedMenuIdList = objMenuAuthorize.Data.Select(p => p.MenuId).ToList();
                //根据用户有权限的菜单ID过滤出其菜单列表
                menuList = menuList.Where(p => authorizedMenuIdList.Contains(p.Id)).ToList();
            }
            // 传送数据到前端
            ViewBag.MenuList = menuList;
            ViewBag.OperatorInfo = operatorInfo;
            return View();
        }

        /// <summary>
        /// Welcome
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Welcome() { return View(); }

        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login() 
        {
            if ( GlobalContext.SystemConfig.Debug)
            {
                ViewBag.UserName = "admin";
                ViewBag.Password = "123456";
            }
            return View(); 
        }

        /// <summary>
        /// No Permission
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult NoPermission() { return View(); }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Error(string message) 
        { 
            ViewBag.Message = message;
            return View(); 
        }
        /// <summary>
        /// Skin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Skin()  {  return View();  }

        #endregion


        /// <summary>
        /// 获取 Captcha 图片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCaptchaImage()
        {
            Tuple<string, string> captchaCode = CaptchaHelper.GetCaptchaCode();
            byte[] bytes = CaptchaHelper.CreateCaptchaImage(captchaCode.Item1);
            //// 验证码结果保存在Session中
            SessionHelper.Set("CaptchaCode", captchaCode.Item2);
            return File(bytes, @"image/jepg");
        }

        /// <summary>
        /// 登录账号
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="captchaCode"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LoginJson(string userName, string password, string captchaCode)
        {
            TData obj = new TData();

            #region 验证Captcha
            if (string.IsNullOrEmpty(captchaCode))
            {
                obj.Message = "验证码不能为空";
                return Json(obj);
            }
            // 验证码结果保存在Session中
            if (captchaCode != SessionHelper.Get("CaptchaCode").ParseToString() )
            {
                obj.Message = "验证码错误，请重新输入";
                return Json(obj);
            }
            #endregion

            TData<UserEntity> userObj = await userBLL.CheckLogin(userName, password, (int)PlatformEnum.Web);
            if (userObj.Tag == 1) //成功
            {
                await new UserBLL().UpdateUser(userObj.Data);
                // 添加 token
                Operator.Instance.AddCurrent(userObj.Data.WebToken);
            }

            obj.Tag = userObj.Tag;
            obj.Message = userObj.Message;
            return Json(obj);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LoginOffJson()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            if (operatorInfo != null)
            {
                // 如果不允许同一个用户多次登录，当用户登出的时候，就不在线了
                if (!GlobalContext.SystemConfig.LoginMultiple)
                {
                    await userBLL.UpdateUser(new UserEntity { Id = operatorInfo.UserId, IsOnline = 0 });
                }
                // 移除 token
                Operator.Instance.RemoveCurrent();
                CookieHelper.Remove("RememberMe");

                return Json(new TData { Tag = 1 });
            }
            else
            {
                throw new Exception("非法请求！");
            }
        }
    }
}
