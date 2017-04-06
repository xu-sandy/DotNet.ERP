using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Logic.ApiData.DataSynchronism;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pharos.Logic.DataSynchronism.ObjectModels
{
    [Table("SaleOrders")]
    public class DataSyncSaleOrders : ISource
    {
        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：10]
        /// [不允许为空]
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// 流水号（全局唯一）
        /// [长度：50]
        /// [不允许为空]
        /// </summary>
        public string PaySN { get; set; }


        /// <summary>
        /// 门店ID
        /// [长度：3]
        /// [不允许为空]
        /// </summary>
        public string StoreId { get; set; }


        /// <summary>
        /// POS机号
        /// [长度：20]
        /// [不允许为空]
        /// </summary>
        public string MachineSN { get; set; }



        /// <summary>
        /// 金额合计（优惠后)
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// [默认值：((0))]
        /// </summary>
        public decimal TotalAmount { get; set; }


        /// <summary>
        /// 优惠合计
        /// [长度：19，小数位数：4]
        /// [不允许为空]
        /// </summary>
        public decimal PreferentialPrice { get; set; }


        /// <summary>
        /// 支付方式ID（多个ID以,号间隔）
        /// [长度：100]
        /// [不允许为空]
        /// </summary>
        public string ApiCode { get; set; }


        /// <summary>
        /// 交易时间
        /// [长度：23，小数位数：3]
        /// [不允许为空]
        /// [默认值：(getdate())]
        /// </summary>
        public DateTime CreateDT { get; set; }


        /// <summary>
        /// 收银员UID 
        /// [长度：40]
        /// </summary>
        public string CreateUID { get; set; }


        /// <summary>
        /// 导购员UID
        /// [长度：40]
        /// </summary>
        public string Salesman { get; set; }


        /// <summary>
        /// 备注
        /// [长度：200]
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 账单类型(0：正常销售；1：换货) 默认值：0
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 状态（默认：0，0：正常，1：已发生退换）
        /// </summary>
        public int State { get; set; }


        /// <summary>
        /// 退换ID
        /// （多个以逗号连接）
        /// </summary>
        public string ReturnId { get; set; }
        /// <summary>
        /// 数据源版本记录
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
