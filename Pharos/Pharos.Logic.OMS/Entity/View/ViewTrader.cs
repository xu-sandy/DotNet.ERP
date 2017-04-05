using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity.View
{
    [Serializable]
     public  class ViewTrader
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public int CID { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 跟进状态
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 客户简称
        /// </summary>
        public string khTitle { get; set; }

        /// <summary>
        /// 客户全称
        /// </summary>
        public string khFullTitle { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public string khType { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public string tType { get; set; }

        /// <summary>
        /// 经营类别
        /// </summary>
        public string bCategory { get; set; }


        /// <summary>
        ///采购需求
        /// </summary>
        public string oList { get; set; }


        /// <summary>
        /// 门店数量
        /// </summary>
        public string ExistStoreNum { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 登记人
        /// </summary>
        public string cFullName { get; set; }

        
    }
}
