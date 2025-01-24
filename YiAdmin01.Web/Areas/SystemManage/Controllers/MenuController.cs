using Microsoft.AspNetCore.Mvc;
using YiAdmin01.BLL.Business;
using YiAdmin01.Entity;
using YiAdmin01.Model.Param;
using YiAdmin01.Model.Result;
using YiAdmin01.Model;
using YiAdmin01.Web.Filters;

namespace YiAdmin01.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class MenuController : Controller
    {
        private MenuBLL menuBLL = new MenuBLL();

        #region 视图View
        [HttpGet]
        [AuthorizeFilter("system:menu:view")]
        public IActionResult MenuIndex()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MenuForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MenuChoose()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MenuIcon()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:menu:search,system:role:search")]
        public async Task<IActionResult> GetListJson(MenuListParam param)
        {
            TData<List<MenuEntity>> obj = await menuBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:menu:search,system:role:search")]
        public async Task<IActionResult> GetMenuTreeListJson(MenuListParam param)
        {
            TData<List<ZtreeInfo>> obj = await menuBLL.GetZtreeList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:menu:view")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<MenuEntity> obj = await menuBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson(long parentId = 0)
        {
            TData<int> obj = await menuBLL.GetMaxSort(parentId);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("system:menu:add,system:menu:edit")]
        public async Task<IActionResult> SaveFormJson(MenuEntity entity)
        {
            TData<string> obj = await menuBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("system:menu:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await menuBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
