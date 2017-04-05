// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有门店的商品售后退换信息
// --------------------------------------------------

using Newtonsoft.Json;
using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using System;
using System.Collections.Generic;

namespace Pharos.Logic.LocalEntity
{
    [Excel("售后退换信息")]

    /// <summary>
    /// 售后退换信息
    /// </summary>
    public class SalesReturns : BaseEntity, ICanUploadEntity, IEqualityComparer<SalesReturns>
    {
        public Int64 Id { get; set; }
        [Excel("退换方式", 1)]
        [ExcelField(@"^[0,1]$###退换方式值范围（0:退货、1:换货）")]

        /// <summary>
        /// 退换方式（0:退货、1:换货、2、整单退出）
        /// [长度：5]
        /// [不允许为空]
        /// </summary>
        public short ReturnType { get; set; }
        [LocalKey]
        [Excel("退换货ID", 2)]
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###退换货ID应为Guid")]


        /// <summary>
        /// 退换货ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string ReturnId { get; set; }
        [Excel("门店ID", 3)]
        [ExcelField(@"^[0-9]{1,2}$###门店ID长度应在1~2位且为数字")]

        /// <summary>
        /// 门店ID
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string StoreId { get; set; }

        public string MachineSN { get; set; }
        [Excel("新单据号", 4)]
        [ExcelField(@"^[0-9]{1,30}$###新单据号最多30位数字")]

        /// <summary>
        /// 新单据号
        ///  [长度：50]
        /// </summary>
        public string NewPaySN { get; set; }
        [Excel("退换理由ID", 5)]
        [ExcelField(@"^[0-9]{1,10}$###退换理由ID长度应在1~10位且为数字")]

        /// <summary>
        /// 退换理由ID（来自数据字典）
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int ReasonId { get; set; }
        [Excel("退换差价", 6)]
        [ExcelField(@"^[0-9]*[.]{0,1}[0-9]*$###退换差价格式错误")]

        /// <summary>
        /// 退换差价
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal ReturnPrice { get; set; }
        [Excel("退换时间", 7)]
        [ExcelField(@"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$###退换时间格式为yyyy-MM-dd HH:mm:ss")]

        /// <summary>
        /// 退换时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }
        [Excel("经办人UID", 8)]
        [ExcelField(@"^[0-9,a-z,A-Z]{1,40}$###经办人UID应为Guid")]


        /// <summary>
        /// 经办人UID 
        /// [长度：40]
        /// [不允许为空]
        /// </summary>
        public string CreateUID { get; set; }
        [Excel("状态", 9)]
        [ExcelField(@"^[1,2]$###状态值范围（1:处理中、2:已完成）")]

        /// <summary>
        /// 状态
        /// </summary>
        public short State { get; set; }
        [JsonIgnore]

        public bool IsUpload { get; set; }

        public bool Equals(SalesReturns x, SalesReturns y)
        {
            return x.ReturnId == y.ReturnId;
        }

        public int GetHashCode(SalesReturns obj)
        {
            return obj.ReturnId.GetHashCode();
        }
    }
}
