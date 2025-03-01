
using YiAdmin01.BLL.Services.InfoService;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity.InfoManage;
using YiAdmin01.Model.Param;
using YiAdmin01.Model;
using YiAdmin01.Common.Extension;
using YiAdmin01.DAL.Enum;
using YiAdmin01.WebCode;
using YiAdmin01.BLL.Services.OrganizationManage;
using YiAdmin01.Entity;


namespace YiAdmin01.BLL.Business.InfoBLL
{
    public class AppointmentBLL
    {
        private AppointmentService appointmentService = new AppointmentService();
        private AppointmentInstanceService appointmentInstanceService = new AppointmentInstanceService();
        private AppointmentRecordService appointmentRecordService = new AppointmentRecordService();
        private AppointmentNodeBLL appointmentNodeBLL = new AppointmentNodeBLL();
        private UserService userService = new UserService();

        #region 获取数据
        public async Task<TData<List<AppointmentEntity>>> GetList(AppointmentListParam param)
        {
            TData<List<AppointmentEntity>> obj = new TData<List<AppointmentEntity>>();
            obj.Data = await appointmentService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AppointmentEntity>>> GetPageList(AppointmentListParam param, Pagination pagination)
        {
            TData<List<AppointmentEntity>> obj = new TData<List<AppointmentEntity>>();
            obj.Data = await appointmentService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AppointmentEntity>> GetEntity(long id)
        {
            TData<AppointmentEntity> obj = new TData<AppointmentEntity>();
            obj.Data = await appointmentService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 保存新建预约单、实例单、记录单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TData<string>> SaveForm(AppointmentEntity entity)
        {
            // 1\保存预约单
            TData<string> obj = new TData<string>();
            entity.AppointmentStatus = AppointmentStatusEnum.Submitted.ParseToInt();
            long AppointmentId = await appointmentService.SaveNewForm(entity);

            // 2\新建并保存预约实例
            var instant = new AppointmentInstanceEntity();
            //当前登录人员信息
            OperatorInfo user = await Operator.Instance.Current(); //amy001
            instant.StarterId = user.UserId;
            //节点
            var nodeList = await appointmentNodeBLL.GetList(null);
            //当前节点
            var currentNodeEntity = nodeList.Data.FirstOrDefault(x => x.CurrentNodeName.Equals("发起预约申请")); //10001
            var nextNodeEntity = nodeList.Data.FirstOrDefault(x => x.CurrentNodeName.Equals(currentNodeEntity.NextNodeName));//10002
            instant.CurrentNodeCode = currentNodeEntity.CurrentNodeCode;
            instant.CurrentNodeName = currentNodeEntity.CurrentNodeName;
            //预约处理状态
            instant.AppointmentStatus = AppointmentStatusEnum.Submitted.ParseToInt();
            //预约申请人（流程发起人）
            instant.Starter = user.UserName;
            instant.StarterId = user.UserId;
            //当前操作者
            instant.OperatorId = user.UserId;
            instant.Operator = user.UserName;
            //下一个操作者
            instant.NextOperator = nextNodeEntity.OperatorName;
            instant.NextOperatorId = nextNodeEntity.OperatorId;
            //预约单ID
            instant.AppointmentId = AppointmentId;
            //已操作过的人
            instant.OperatedNames = user.UserName;
            long instantId = await appointmentInstanceService.SaveNewForm(instant);

            // 2\新建预约流转记录
            var record = new AppointmentRecordEntity();

            //当前处理人
            record.OperatorId = user.UserId;
            record.OperatorName = user.UserName;
            //当前节点
            record.CurrentNodeCode = instant.CurrentNodeCode;
            record.CurrentNodeName = instant.CurrentNodeName;
            //审批状态
            record.ApprovedStatus = ApprovedStatusEnum.Unapproved.ParseToInt();
            //是否通过
            record.ResultStatus = ResultStatusEnum.Unprocessed.ParseToInt();
            //实例Id          
            record.InstanceId = instantId;

            await appointmentRecordService.SaveForm(record);

            //可在此调用邮件服务发送邮件通知//
            #region 发送邮件通知
            UserEntity userEntity = await userService.GetEntity(instant.Starter);
            MailInfo mailInfo1 = new MailInfo{
                Subject = "预约状态变更提醒",
                ReceiverName = userEntity.UserName,
                Receivers = userEntity.Email,
                Body = "您的预约单已提交，请等待审批！",
            };
            SendEmail(mailInfo1);
            UserEntity userEntity2 = await userService.GetEntity(instant.NextOperator);
            MailInfo mailInfo2 = new MailInfo
            {
                Subject = "有预约单需要审批",
                ReceiverName = userEntity2.UserName,
                Receivers = userEntity2.Email,
                Body = "有预约单已提交，请您审批！",
            };
            
            SendEmail(mailInfo2);
            #endregion

            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

       
        /// <summary>
        /// 保存审批后的预约单、实例单、记录单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TData<string>> SaveApprovaledForm(AppointmentEntity entity)
        {
            TData<string> obj = new TData<string>();
            // 1.获取预约实例           
            List<AppointmentInstanceEntity> instanceList = await appointmentInstanceService.GetList(new AppointmentInstanceListParam { AppointmentId = entity.Id.Value });
            var instant = instanceList.FirstOrDefault();
            //当前登录人员信息
            OperatorInfo user = await Operator.Instance.Current();//wh001
            // 节点列表
            var nodeList = await appointmentNodeBLL.GetList(null);
            // 前节点                
            var currentNodeEntity = nodeList.Data.FirstOrDefault(x => x.CurrentNodeCode.Equals(instant.CurrentNodeCode));//10001            

            //2.新建预约流转记录,保存审批记录
            var record = new AppointmentRecordEntity();            
            record.InstanceId = instant.Id;       
            record.OperatorId = user.UserId;
            record.OperatorName = user.UserName;
            record.CurrentNodeCode = currentNodeEntity.NextNodeCode;
            record.CurrentNodeName = currentNodeEntity.NextNodeName;
            //审批状态
            #region 审批状态
            switch (entity.AppointmentStatus)
            {               
                case (int)AppointmentStatusEnum.Agreed:
                case (int)AppointmentStatusEnum.Accepted:
                    record.ApprovedStatus = ApprovedStatusEnum.Approved.ParseToInt();
                    record.ResultStatus = ResultStatusEnum.Agreed.ParseToInt();
                    break;
                case (int)AppointmentStatusEnum.Rejected:
                    record.ApprovedStatus = ApprovedStatusEnum.Approved.ParseToInt();
                    record.ResultStatus = ResultStatusEnum.Rejected.ParseToInt();
                    break;
                case (int)AppointmentStatusEnum.Unaccepted:
                    record.ApprovedStatus = ApprovedStatusEnum.Approved.ParseToInt();
                    record.ResultStatus = ResultStatusEnum.Rejected.ParseToInt();
                    break;
                default: 
                    record.ApprovedStatus = ApprovedStatusEnum.Unapproved.ParseToInt();
                    record.ResultStatus = ResultStatusEnum.Unprocessed.ParseToInt();
                    break;
            }
            #endregion

            await appointmentRecordService.SaveForm(record);
            //保存审批后的预约单
            await appointmentService.SaveForm(entity);
            //3.修改预约实例
            if (entity.AppointmentStatus == AppointmentStatusEnum.Agreed.ParseToInt())
            { //3.1 同意
                //修改实例单、流向下一个节点          
                instant.OperatorId = user.UserId;
                instant.Operator = user.UserName;                
                //实例中当前节点变为下一个节点
                instant.CurrentNodeCode = currentNodeEntity.NextNodeCode;
                instant.CurrentNodeName = currentNodeEntity.NextNodeName;
                //当前操作者
                instant.OperatorId = user.UserId;
                instant.Operator = user.UserName;
                // 节点
                AppointmentNodeEntity nextNodeEntity = new(); 
                if (instant.CurrentNodeCode != null && instant.CurrentNodeCode != 0)
                {
                    nextNodeEntity = nodeList.Data.FirstOrDefault(x => x.PrevNodeCode.Equals(instant.CurrentNodeCode));
                }
                //下一个操作者
                instant.NextOperator = nextNodeEntity.OperatorName ?? " ";
                instant.NextOperatorId = nextNodeEntity.OperatorId ?? 0;
            }
            else if (entity.AppointmentStatus == AppointmentStatusEnum.Rejected.ParseToInt()
                || entity.AppointmentStatus == AppointmentStatusEnum.Unaccepted.ParseToInt())
            { //3.2 拒绝
                //修改实例单、流向预约发起者
                instant.OperatorId = instant.StarterId;
                instant.Operator = instant.Starter;
                //起始节点
                var startNodeEntity = nodeList.Data.FirstOrDefault(x => x.CurrentNodeName.Equals("发起预约申请"));
                instant.CurrentNodeCode = startNodeEntity.CurrentNodeCode;
                instant.CurrentNodeName = startNodeEntity.CurrentNodeName;
                //当前操作者变成预约发起者
                instant.OperatorId = startNodeEntity.OperatorId;
                instant.Operator = startNodeEntity.OperatorName;
                //下一个操作者,变更为预约发起者
                instant.NextOperator = startNodeEntity.OperatorName;
                instant.NextOperatorId = startNodeEntity.OperatorId;  
            }
            else if (entity.AppointmentStatus == AppointmentStatusEnum.Accepted.ParseToInt())
            { // 3.3 收货
                //修改实例单、流向下一个节点          
                instant.OperatorId = user.UserId; //wh002
                instant.Operator = user.UserName;
                //实例中当前节点变为下一个节点
                instant.CurrentNodeCode = currentNodeEntity.NextNodeCode ?? 0;
                instant.CurrentNodeName = currentNodeEntity.NextNodeName ?? " ";
                //当前操作者
                instant.OperatorId = user.UserId;
                instant.Operator = user.UserName;
                // 节点
                AppointmentNodeEntity nextNodeEntity = null;
                if (instant.CurrentNodeCode != null && instant.CurrentNodeCode != 0)
                {
                    nextNodeEntity = nodeList.Data.FirstOrDefault(x => x.PrevNodeCode.Equals(instant.CurrentNodeCode));
                }
                //下一个操作者
                if( nextNodeEntity != null)
                {
                    instant.NextOperator = nextNodeEntity.OperatorName ?? " ";
                    instant.NextOperatorId = nextNodeEntity.OperatorId ?? 0;

                }
                instant.NextOperator =   " ";
                instant.NextOperatorId =  0;
            }            
            else
            {
            }

            //已操作过的人
            instant.OperatedNames += ", " + user.UserName.ParseToString();
            //预约处理状态
            instant.AppointmentStatus = entity.AppointmentStatus;
            //保存预约实例 
            //await appointmentInstanceService.SaveForm(instant);
            AppointmentInstanceEntity resultInstance = await appointmentInstanceService.SaveExsitForm(instant);


            // 可在此调用邮件服务发送邮件通知//

            #region 发送邮件通知
            UserEntity userEntity1 = await userService.GetEntity(resultInstance.Starter);
            MailInfo mailInfoStarter = new MailInfo
            {
                Subject = "您的预约单状态已变更为：" + ((AppointmentStatusEnum)resultInstance.AppointmentStatus).GetDescription(),
                ReceiverName = userEntity1.UserName,
                Receivers = userEntity1.Email,
                Body = "您的预约单状态已变更为：" + ((AppointmentStatusEnum)resultInstance.AppointmentStatus).GetDescription(),

            };
            SendEmail(mailInfoStarter);
            
            if (!string.IsNullOrWhiteSpace(resultInstance.NextOperator) && !resultInstance.Starter.Equals(resultInstance.NextOperator))
            {
                UserEntity userEntity2 = await userService.GetEntity(resultInstance.NextOperator);
                if(userEntity2 != null)
                {
                    MailInfo mailInfoNext = new MailInfo
                    {
                        Subject = "您有待审批预约",
                        ReceiverName = userEntity2.UserName,
                        Receivers = userEntity2.Email,
                        Body = "您有待审批预约,请您审批！",
                        CC = "",
                    };
                    SendEmail(mailInfoNext);
                }
                
            }
            
            #endregion

            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await appointmentService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> ImportAppointment(ImportParam param, List<AppointmentEntity> list)
        {
            TData obj = new TData();
            if (list.Count != 0)
            {
                foreach (AppointmentEntity entity in list)
                {
                    AppointmentEntity dbEntity = await appointmentService.GetEntity(entity.Id.Value);
                    if (dbEntity != null)
                    {
                        entity.Id = dbEntity.Id;
                        if (param.IsOverride == 1)
                        {
                            await appointmentService.SaveForm(entity);
                        }
                    }
                    else
                    {
                        await appointmentService.SaveForm(entity);
                    }
                }
                obj.Tag = 1;
            }
            else
            {
                obj.Message = " 未找到导入的数据";
            }
            return obj;
        }
        #endregion

        #region 私有方法
        private async void SendEmail(MailInfo mailInfo)
        {
            var emailHelper = new EmailHelper(EmailServiceConfig._126Mail(), mailInfo);
            bool result = await emailHelper.SendMail();
            if (result)
            {
                Console.WriteLine("邮件发送成功,"+mailInfo.Receivers+","+mailInfo.Body);
            }
            else
            {
                Console.WriteLine("邮件发送失败" + mailInfo.Receivers + "," + mailInfo.Body);
            }
            emailHelper.smtpClient.Disconnect(true);
        }
        #endregion
    }
}
