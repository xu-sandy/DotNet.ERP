using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.Printer
{
    public class DayReportModel
    {
        #region 发票格式参数
        /// <summary>
        /// 发票宽度，按字符数计算，根据打印机型号有所区别(通常在30-70之间)
        /// </summary>
        public int TicketWidth { get; set; }
        #endregion
        /// <summary>
        /// 门店名称
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 第二行标题
        /// </summary>
        public string Title2 { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        public string StockDateStr { get; set; }
        /// <summary>
        /// 打印时间
        /// </summary>
        public DateTime PrintDate { get; set; }
        /// <summary>
        /// 交易项目
        /// </summary>
        public List<TransactionItemModel> TransactionItemList { get; set; }
        /// <summary>
        /// 员工列表
        /// </summary>
        public List<EmployeeModel> EmployeeList { get; set; }
    }
}
