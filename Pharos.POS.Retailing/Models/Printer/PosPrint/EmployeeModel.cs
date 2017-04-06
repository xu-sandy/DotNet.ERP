using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.Printer
{
    public class EmployeeModel
    {
        /// <summary>
        /// 工号
        /// </summary>
        public string EmployeeSN { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 首笔时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 末笔时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 交易条目
        /// </summary>
        public List<TransactionItemModel> EmployeeTransactionItems { get; set; }
            
    }
}
