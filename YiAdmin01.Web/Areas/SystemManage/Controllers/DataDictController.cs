using Microsoft.AspNetCore.Mvc;
using YiAdmin01.BLL.Business;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity;
using YiAdmin01.Model.Param;
using YiAdmin01.Model.Result;
using YiAdmin01.Model;
using YiAdmin01.Web.Filters;

namespace YiAdmin01.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 数据字典
    /// refer - yisha-data-js
    /// </summary>
    [Area("SystemManage")]
    public class DataDictController : Controller
    {
        private DataDictBLL dataDictBLL = new DataDictBLL();

        #region View
        [HttpGet]
        [AuthorizeFilter("system:datadict:view")]
        public IActionResult DataDictIndex()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DataDictForm()
        {
            return View();
        }

        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:datadict:search")]
        public async Task<IActionResult> GetListJson(DataDictListParam param)
        {
            TData<List<DataDictEntity>> obj = await dataDictBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:datadict:search")]
        public async Task<IActionResult> GetPageListJson(DataDictListParam param, Pagination pagination)
        {
            TData<List<DataDictEntity>> obj = await dataDictBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:datadict:view")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<DataDictEntity> obj = await dataDictBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            TData<int> obj = await dataDictBLL.GetMaxSort();
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:datadict:view")]
        public async Task<IActionResult> GetDataDictListJson()
        {
            TData<List<DataDictInfo>> obj = await dataDictBLL.GetDataDictList();
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("system:datadict:add,system:datadict:edit")]
        public async Task<IActionResult> SaveFormJson(DataDictEntity entity)
        {
            TData<string> obj = await dataDictBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("system:datadict:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await dataDictBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion

    }
}
