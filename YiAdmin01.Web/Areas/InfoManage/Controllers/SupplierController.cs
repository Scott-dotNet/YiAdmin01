using Microsoft.AspNetCore.Mvc;
using YiAdmin01.BLL.Business.InfoBLL;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;
using YiAdmin01.Model;
using YiAdmin01.Web.Controllers;
using YiAdmin01.Web.Filters;
using YiAdmin01.BLL.Business;
using YiAdmin01.Model.Result;
using YiAdmin01.Entity;
using YiAdmin01.Common.Utils;

namespace YiAdmin01.Web.Areas.InfoManage.Controllers
{
    /// <summary>
    /// 供应商控制器类
    /// </summary>
    /// 
    [Area("InfoManage")]
    public class SupplierController : Controller
    {
        private SupplierBLL supplierBLL = new SupplierBLL();

        #region 视图功能
        [HttpGet]
        [AuthorizeFilter("info:supplier:view")]
        public ActionResult SupplierIndex()
        {
            return View();
        }

        [HttpGet]
        //[AuthorizeFilter("info:supplier:view")]
        public ActionResult SupplierForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("info:supplier:search")]
        public async Task<ActionResult> GetListJson(SupplierListParam param)
        {
            TData<List<SupplierEntity>> obj = await supplierBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("info:supplier:search")]
        public async Task<ActionResult> GetPageListJson(SupplierListParam param, Pagination pagination)
        {
            TData<List<SupplierEntity>> obj = await supplierBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("info:supplier:view")]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<SupplierEntity> obj = await supplierBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("info:supplier:view")]
        public async Task<IActionResult> GetSupplierNameJson(SupplierListParam param)
        {
            TData<string> obj = new TData<string>();
            var list = await supplierBLL.GetList(param);
            if (list.Tag == 1)
            {
                obj.Data = string.Join(",", list.Data.Select(p => p.CompanyCnName));
                obj.Tag = 1;
            }
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("info:supplier:add,info:supplier:edit")]
        public async Task<ActionResult> SaveFormJson(SupplierEntity entity)
        {
            TData<string> obj = await supplierBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("info:supplier:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await supplierBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:user:edit")]
        public async Task<IActionResult> ExportSupplierJson(SupplierListParam param)
        {
            TData<string> obj = new TData<string>();
            TData<List<SupplierEntity>> supplierObj = await supplierBLL.GetList(param);
            if (supplierObj.Tag == 1)
            {
                string fileName = $"供应商列表_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xls";
                string[] fileCols = { "CompanyCnName", "ContactPerson", "Tel", "Fax", "Email" };
                string file = new ExcelHelper<SupplierEntity>().ExportToExcel( fileName,
                                                                          "供应商列表",
                                                                          supplierObj.Data,
                                                                          fileCols
                                                                          );
                obj.Data = file;
                obj.Tag = 1;
            }
            return Json(obj);
        }
        #endregion
    }
}
