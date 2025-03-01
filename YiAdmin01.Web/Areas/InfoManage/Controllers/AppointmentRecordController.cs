using Microsoft.AspNetCore.Mvc;
using YiAdmin01.BLL.Business.InfoBLL;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;
using YiAdmin01.Model;
using YiAdmin01.Web.Controllers;
using YiAdmin01.Web.Filters;

namespace YiAdmin01.Web.Areas.InfoManage.Controllers
{
    [Area("InfoManage")]
    public class AppointmentRecordController : Controller
    {
        private AppointmentRecordBLL appointmentRecordBLL = new AppointmentRecordBLL();

        #region 视图功能
        
        [HttpGet]
        [AuthorizeFilter("info:appointmentrecord:view")]     
        public ActionResult AppointmentRecordIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AppointmentRecordForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("info:appointmentrecord:search")]
        public async Task<ActionResult> GetListJson(AppointmentRecordListParam param)
        {
            TData<List<AppointmentRecordEntity>> obj = await appointmentRecordBLL.GetList(param);
            return Json(obj);
        }

        
        [HttpGet]
        [AuthorizeFilter("info:appointmentrecord:search")]
        public async Task<ActionResult> GetPageListJson(AppointmentRecordListParam param, Pagination pagination)
        {
            TData<List<AppointmentRecordEntity>> obj = await appointmentRecordBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<AppointmentRecordEntity> obj = await appointmentRecordBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("info:appointmentrecord:add,info:appointmentrecord:edit")]
        public async Task<ActionResult> SaveFormJson(AppointmentRecordEntity entity)
        {
            TData<string> obj = await appointmentRecordBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("info:appointmentrecord:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await appointmentRecordBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
