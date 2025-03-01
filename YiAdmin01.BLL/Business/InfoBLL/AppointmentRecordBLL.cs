using YiAdmin01.BLL.Services.InfoService;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;
using YiAdmin01.Model;
using YiAdmin01.Common.Extension;
using YiAdmin01.WebCode;

namespace YiAdmin01.BLL.Business.InfoBLL
{
    public class AppointmentRecordBLL
    {
        private AppointmentRecordService appointmentRecordService = new AppointmentRecordService();

        #region 获取数据
        public async Task<TData<List<AppointmentRecordEntity>>> GetList(AppointmentRecordListParam param)
        {
            TData<List<AppointmentRecordEntity>> obj = new TData<List<AppointmentRecordEntity>>();
            obj.Data = await appointmentRecordService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AppointmentRecordEntity>>> GetPageList(AppointmentRecordListParam param, Pagination pagination)
        {
            
            TData<List<AppointmentRecordEntity>> obj = new TData<List<AppointmentRecordEntity>>();
            obj.Data = await appointmentRecordService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AppointmentRecordEntity>> GetEntity(long id)
        {
            TData<AppointmentRecordEntity> obj = new TData<AppointmentRecordEntity>();
            obj.Data = await appointmentRecordService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(AppointmentRecordEntity entity)
        {
            TData<string> obj = new TData<string>();
            await appointmentRecordService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await appointmentRecordService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
