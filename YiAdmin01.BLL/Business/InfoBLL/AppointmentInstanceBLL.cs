
using YiAdmin01.BLL.Services.InfoService;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;
using YiAdmin01.Model;
using YiAdmin01.Common.Extension;
using YiAdmin01.WebCode;

namespace YiAdmin01.BLL.Business.InfoBLL
{
    public class AppointmentInstanceBLL
    {
        private AppointmentInstanceService appointmentInstanceService = new AppointmentInstanceService();

        #region 获取数据
        public async Task<TData<List<AppointmentInstanceEntity>>> GetList(AppointmentInstanceListParam param)
        {
            // 获取待办审批：
            var recordList = await appointmentInstanceService.GetList(param);
            OperatorInfo user = await Operator.Instance.Current();
            //过滤,根据预约实例中的 NextOperatorId 来进行获取，若NextOperatorId为当前登录的用户，则获取这个待办事项；
            var filtedRecordList = recordList.Where(t => t.NextOperatorId == user.UserId).ToList();

            TData<List<AppointmentInstanceEntity>> obj = new TData<List<AppointmentInstanceEntity>>();
            obj.Data = await appointmentInstanceService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        /// 获取分页待审批数据
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<TData<List<AppointmentInstanceEntity>>> GetPageList(AppointmentInstanceListParam param, Pagination pagination)
        {
            // 获取待办审批：
            var recordList = await appointmentInstanceService.GetPageList(param, pagination);
            OperatorInfo user = await Operator.Instance.Current();
            //过滤,根据预约实例中的 NextOperatorId 来进行获取，若NextOperatorId为当前登录的用户，则获取这个待办事项；
            var filtedRecordList = recordList.Where(t => t.NextOperatorId == user.UserId).ToList();

            TData<List<AppointmentInstanceEntity>> obj = new TData<List<AppointmentInstanceEntity>>();
            obj.Data = filtedRecordList;
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AppointmentInstanceEntity>> GetEntity(long id)
        {
            TData<AppointmentInstanceEntity> obj = new TData<AppointmentInstanceEntity>();
            obj.Data = await appointmentInstanceService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(AppointmentInstanceEntity entity)
        {
            TData<string> obj = new TData<string>();
            await appointmentInstanceService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await appointmentInstanceService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}
