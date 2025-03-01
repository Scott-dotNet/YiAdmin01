using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiAdmin01.DAL.Enum
{
    public enum AppointmentEnum
    {
        
    }
    /// <summary>
    /// 货物类型
    /// </summary>
    public enum CargoTypeEnum
    {
        [Description("普通货物")]
        Common = 1,

        [Description("危险品")]
        Danger = 2,
    }

    /// <summary>
    /// 重量单位
    /// </summary>
    public enum WeightUnitEnum
    {
        [Description("吨")]
        Tons = 1,

        [Description("千克")]
        Kilograms = 2,

        [Description("克")]
        grams = 3
    }

    /// <summary>
    /// 到货时间
    /// </summary>
    public enum ArrivalTimeEnum
    {
        [Description("上午")]
        Morning = 1,

        [Description("下午")]
        Afternoon = 2,

        [Description("晚上")]
        Evening = 3
    }

    /// <summary>
    /// 预约状态
    /// </summary>
    public enum AppointmentStatusEnum
    {
        [Description("提交预约")]
        Submitted = 1,

        [Description("同意预约")]
        Agreed = 2,

        [Description("拒绝预约")]
        Rejected = 12,               

        [Description("已收货")]
        Accepted = 3,

        [Description("已拒收")]
        Unaccepted = 13

    }

    /// <summary>
    /// 结果状态
    /// </summary>
    public enum ResultStatusEnum
    {
        [Description("已同意")]
        Agreed = 1,

        [Description("已拒绝")]
        Rejected = 0,

        [Description("未处理")]
        Unprocessed = 2

    }

    /// <summary>
    /// 审批状态
    /// </summary>
    public enum ApprovedStatusEnum
    {
        [Description("已审批")]
        Approved = 1,

        [Description("未审批")]
        Unapproved = 0,

    }
}
