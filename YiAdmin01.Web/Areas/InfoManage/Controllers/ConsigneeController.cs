using Microsoft.AspNetCore.Mvc;
using YiAdmin01.BLL.Business.InfoBLL;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;
using YiAdmin01.Model;
using YiAdmin01.Web.Controllers;
using YiAdmin01.Web.Filters;
using YiAdmin01.BLL.Business;
using YiAdmin01.Common.Utils;
using YiAdmin01.Entity;

namespace YiAdmin01.Web.Areas.InfoManage.Controllers
{
    /// <summary>
    /// 货主控制器类
    /// </summary>
    [Area("InfoManage")]
    public class ConsigneeController : BaseController
    {
        private ConsigneeBLL consigneeBLL = new ConsigneeBLL();

        #region 视图功能
        [HttpGet]
        [AuthorizeFilter("info:consignee:view")]
        public ActionResult ConsigneeIndex()
        {
            return View();
        }

        [HttpGet]
        //[AuthorizeFilter("info:consignee:view")]
        public ActionResult ConsigneeForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserImport()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("info:consignee:search")]
        public async Task<ActionResult> GetListJson(ConsigneeListParam param)
        {
            TData<List<ConsigneeEntity>> obj = await consigneeBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("info:consignee:search")]
        public async Task<ActionResult> GetPageListJson(ConsigneeListParam param, Pagination pagination)
        {
            TData<List<ConsigneeEntity>> obj = await consigneeBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("info:consignee:view")]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<ConsigneeEntity> obj = await consigneeBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("info:consignee:view")]
        public async Task<IActionResult> GetConsigneeNameJson(ConsigneeListParam param)
        {
            TData<string> obj = new TData<string>();
            var list = await consigneeBLL.GetList(param);
            if (list.Tag == 1)
            {
                obj.Data = string.Join(",", list.Data.Select(p => p.ConsigneeName));
                obj.Tag = 1;
            }
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("info:consignee:add,info:consignee:edit")]
        public async Task<ActionResult> SaveFormJson(ConsigneeEntity entity)
        {
            TData<string> obj = await consigneeBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("info:consignee:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await consigneeBLL.DeleteForm(ids);
            return Json(obj);
        }

        /// <summary>
        /// 导入 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("info:consignee:edit")]
        public async Task<IActionResult> ImportConsigneeJson(ImportParam param)
        {
            List<ConsigneeEntity> list = new ExcelHelper<ConsigneeEntity>().ImportFromExcel(param.FilePath);
            TData obj = await consigneeBLL.ImportConsignee(param, list);
            return Json(obj);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter("info:consignee:edit")]
        public async Task<IActionResult> ExportConsigneeJson(ConsigneeListParam param)
        {
            TData<string> obj = new();
            TData<List<ConsigneeEntity>> userObj = await consigneeBLL.GetList(param);
            if (userObj.Tag == 1)
            {                
                string[] columns = { "CompanyCnName", "CnAddress", "ContactPerson", "Tel", "Fax", "Email" };
                string file = new ExcelHelper<ConsigneeEntity>().ExportToExcel("货主列表.xls",
                                                                          "货主列表",
                                                                          userObj.Data,
                                                                          columns);
                obj.Data = file;
                obj.Tag = 1;
            }
            return Json(obj);
        }
        #endregion
    }
}
