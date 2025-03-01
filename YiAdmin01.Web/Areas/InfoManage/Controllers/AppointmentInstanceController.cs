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
    public class AppointmentInstanceController : Controller
    {
        private AppointmentInstanceBLL appointmentInstanceBLL = new AppointmentInstanceBLL();

        #region 视图功能
        [AuthorizeFilter("info:appointmentinstance:view")]
        public ActionResult AppointmentInstanceIndex()
        {
            return View();
        }

        public ActionResult AppointmentInstanceForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("info:appointmentinstance:search")]
        public async Task<ActionResult> GetListJson(AppointmentInstanceListParam param)
        {
            TData<List<AppointmentInstanceEntity>> obj = await appointmentInstanceBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("info:appointmentinstance:search")]
        public async Task<ActionResult> GetPageListJson(AppointmentInstanceListParam param, Pagination pagination)
        {
            TData<List<AppointmentInstanceEntity>> obj = await appointmentInstanceBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<AppointmentInstanceEntity> obj = await appointmentInstanceBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("info:appointmentinstance:add,info:appointmentinstance:edit")]
        public async Task<ActionResult> SaveFormJson(AppointmentInstanceEntity entity)
        {
            TData<string> obj = await appointmentInstanceBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("info:appointmentinstance:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await appointmentInstanceBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
