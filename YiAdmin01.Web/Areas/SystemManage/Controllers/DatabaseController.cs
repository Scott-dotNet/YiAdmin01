using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;
using YiAdmin01.Common.Configs;
using YiAdmin01.Model;
using YiAdmin01.Web.Controllers;
using YiAdmin01.Web.Filters;
using YiAdmin01.Model.Result;
using YiAdmin01.BLL.Business;

namespace YiAdmin01.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class DatabaseController : Controller
    {
        private DatabaseTableBLL databaseTableBLL = new DatabaseTableBLL();

        #region 视图功能
        [AuthorizeFilter("system:datatable:view")]
        public IActionResult DatatableIndex()
        {
            return View();
        }
        public IActionResult AreaForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:datatable:search")]
        public async Task<IActionResult> GetTableListJson(string tableName)
        {
            TData<List<SqlTableInfo>> obj = await databaseTableBLL.GetTableList(tableName);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:datatable:search")]
        public async Task<IActionResult> GetTablePageListJson(string tableName, Pagination pagination)
        {
            TData<List<SqlTableInfo>> obj = await databaseTableBLL.GetTablePageList(tableName, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:datatable:view")]
        public async Task<IActionResult> GetTableFieldListJson(string tableName)
        {
            TData<List<TableFieldInfo>> obj = await databaseTableBLL.GetTableFieldList(tableName);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        //[HttpPost]
        //public async Task<IActionResult> SyncDatabaseJson()
        //{
        //    TData obj = await databaseTableBLL.SyncDatabase();
        //    return Json(obj);
        //}
        #endregion
    }
}
