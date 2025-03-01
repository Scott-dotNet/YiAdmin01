using YiAdmin01.BLL.Services.InfoService;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;
using YiAdmin01.Model;
using YiAdmin01.Common.Extension;

namespace YiAdmin01.BLL.Business.InfoBLL
{
    public class AppointmentNodeBLL
    {
        private AppointmentNodeService appointmentNodeService = new AppointmentNodeService();

        #region 获取数据
        public async Task<TData<List<AppointmentNodeEntity>>> GetList(AppointmentNodeListParam param)
        {
            TData<List<AppointmentNodeEntity>> obj = new TData<List<AppointmentNodeEntity>>();
            obj.Data = await appointmentNodeService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AppointmentNodeEntity>>> GetPageList(AppointmentNodeListParam param, Pagination pagination)
        {
            TData<List<AppointmentNodeEntity>> obj = new TData<List<AppointmentNodeEntity>>();
            obj.Data = await appointmentNodeService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AppointmentNodeEntity>> GetEntity(long id)
        {
            TData<AppointmentNodeEntity> obj = new TData<AppointmentNodeEntity>();
            obj.Data = await appointmentNodeService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(AppointmentNodeEntity entity)
        {
            TData<string> obj = new TData<string>();
            await appointmentNodeService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await appointmentNodeService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}
