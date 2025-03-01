using Microsoft.AspNetCore.Mvc;
using YiAdmin01.BLL.Business.InfoBLL;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;
using YiAdmin01.Model;
using YiAdmin01.Web.Controllers;
using YiAdmin01.Web.Filters;
using YiAdmin01.Common.Utils;
using YiAdmin01.WebCode;
using YiAdmin01.DAL.Enum;
using YiAdmin01.Common.Extension;
using NPOI.HSSF.Record;

namespace YiAdmin01.Web.Areas.InfoManage.Controllers
{
    [Area("InfoManage")]
    public class AppointmentController : Controller
    {
        private AppointmentBLL appointmentBLL = new AppointmentBLL();

        #region 视图功能
        [HttpGet]
        [AuthorizeFilter("info:appointment:view")]
        public ActionResult AppointmentIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AppointmentForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AppointmentImport()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("info:appointment:search")]
        public async Task<ActionResult> GetListJson(AppointmentListParam param)
        {
            TData<List<AppointmentEntity>> obj = await appointmentBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("info:appointment:search")]
        public async Task<ActionResult> GetPageListJson(AppointmentListParam param, Pagination pagination)
        {
            TData<List<AppointmentEntity>> obj = await appointmentBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<AppointmentEntity> obj = await appointmentBLL.GetEntity(id);
            return Json(obj);
        }
        
        [HttpPost]
        [AuthorizeFilter("info:appointment:add,info:appointment:edit")]
        public async Task<ActionResult> SaveFormJson(AppointmentEntity entity)
        {
            // 保存新建预约单
            TData<string> obj = await appointmentBLL.SaveForm(entity);         
          
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("info:appointment:add,info:appointment:edit")]
        public async Task<ActionResult> SaveApprovaledFormJson(AppointmentEntity entity)
        {
            // 保存审批后的预约单
            TData<string> obj = await appointmentBLL.SaveApprovaledForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("info:appointment:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await appointmentBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("info:appointment:edit")]
        public async Task<IActionResult> ImportAppointmentJson(ImportParam param)
        {
            List<AppointmentEntity> list = new ExcelHelper<AppointmentEntity>().ImportFromExcel(param.FilePath);
            TData obj = await appointmentBLL.ImportAppointment(param, list);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("info:appointment:edit")]
        public async Task<IActionResult> ExportSupplierJson(AppointmentListParam param)
        {
            TData<string> obj = new TData<string>();
            TData<List<AppointmentEntity>> supplierObj = await appointmentBLL.GetList(param);
            if (supplierObj.Tag == 1)
            {
                string fileName = $"供应商列表_.xls";
                string[] fileCols = { "BaseCreatorId", "CargoName", "CargoType", "ArrivalTime" };
                string file = new ExcelHelper<AppointmentEntity>().ExportToExcel(fileName,
                                                                          "预约列表",
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
