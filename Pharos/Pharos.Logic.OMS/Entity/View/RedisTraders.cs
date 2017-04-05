using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    [Serializable]
    public class RedisTraders
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客户来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 客户简称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 客户全称
        /// </summary>
        public string FullTitle { get; set; }

        /// <summary>
        /// 街道地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; set; }

        /// <summary>
        /// 电话/手机
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public string BusinessModeId { get; set; }

        /// <summary>
        /// 经营类别
        /// </summary>
        public string BusinessScopeId { get; set; }

        /// <summary>
        /// 现有系统名称/品牌
        /// </summary>
        public string ExistsystemName { get; set; }

        /// <summary>
        /// 现有设备名称/品牌
        /// </summary>
        public string ExistDeviceName { get; set; }

        /// <summary>
        /// 现有门店数量
        /// </summary>
        public string ExistStoreNum { get; set; }

        /// <summary>
        /// 每门店收银电脑台数
        /// </summary>
        public string EachStorePosNum { get; set; }

        /// <summary>
        /// 每个门店人均数
        /// [长度：5]
        /// </summary>
        public string EachStorePersonNum { get; set; }

        /// <summary>
        /// 近2年计划扩张门店数量
        /// </summary>
        public string PlanExpandStoreNum { get; set; }

        /// <summary>
        /// 跟进状态
        /// </summary>
        public string TrackStautsId { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        public string AssignerUID { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string Pay { get; set; }

        /// <summary>
        /// 所在区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 竞争对手
        /// </summary>
        public string Rival { get; set; }

        /// <summary>
        /// 竞争对手的营销模式
        /// </summary>
        public string Marketing { get; set; }

    }
}
