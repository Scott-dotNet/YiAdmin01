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
    public class AppointmentNodeController : Controller
    {
        private AppointmentNodeBLL appointmentNodeBLL = new AppointmentNodeBLL();

        #region 视图功能
        [HttpGet]
        [AuthorizeFilter("info:appointmentnode:view")]
        public ActionResult AppointmentNodeIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AppointmentNodeForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("info:appointmentnode:search")]
        public async Task<ActionResult> GetListJson(AppointmentNodeListParam param)
        {
            TData<List<AppointmentNodeEntity>> obj = await appointmentNodeBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("info:appointmentnode:search")]
        public async Task<ActionResult> GetPageListJson(AppointmentNodeListParam param, Pagination pagination)
        {
            TData<List<AppointmentNodeEntity>> obj = await appointmentNodeBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<AppointmentNodeEntity> obj = await appointmentNodeBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("info:appointmentnode:add,info:appointmentnode:edit")]
        public async Task<ActionResult> SaveFormJson(AppointmentNodeEntity entity)
        {
            TData<string> obj = await appointmentNodeBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("info:appointmentnode:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await appointmentNodeBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
