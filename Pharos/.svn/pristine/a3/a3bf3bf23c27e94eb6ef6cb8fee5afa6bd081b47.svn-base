// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有门店POS机的出入款信息
// --------------------------------------------------

using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.LocalServices;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;
using Newtonsoft.Json;

namespace Pharos.Logic.LocalEntity
{
    [Excel("POS出入款信息")]

    /// <summary>
    /// POS出入款信息
    /// </summary>
    public class PosIncomePayout : BaseEntity, ICanUploadEntity
    {
        public Int64 Id { get; set; }
        [Excel("门店ID",1)]
        [ExcelField(@"^[0-9]{1,2}$###门店ID长度应在1-2位且为数字")]

        [LocalKey]

        /// <summary>
        /// 门店ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string StoreId { get; set; }
        [LocalKey]
        [Excel("POS机号",2)]

        /// <summary>
        /// POS机号
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        public string MachineSN { get; set; }
        [Excel("收银员",3)]
        [LocalKey]

        /// <summary>
        /// 收银员UID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }
        [Excel("类型",4)]
        [LocalKey]

        /// <summary>
        /// 类型（0:出款、1:入款） 
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short Type { get; set; }
        [Excel("金额",5)]
        [LocalKey]

        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###金额格式错误")]

        /// <summary>
        /// 金额
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal Amount { get; set; }
        [LocalKey]
        [Excel("时间",6)]

        /// <summary>
        /// 时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }
        [JsonIgnore]
        public bool IsUpload { get; set; }
    }
}
