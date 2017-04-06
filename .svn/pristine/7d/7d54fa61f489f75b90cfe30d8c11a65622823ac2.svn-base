using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Wpf.ViewModelHelpers;
using Pharos.POS.Retailing.Models.ViewModels;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    public class UserInfoResult
    {
        public SaleManViewModel parent;
        public string UID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 性别（ 0:女、 1:男）
        /// </summary>
        public bool Sex { get; set; }
        /// <summary>
        /// 最近登录时间
        /// </summary>
        public DateTime LoginDT { get; set; }
        /// <summary>
        /// 门店名称（即出货仓）
        /// </summary>
        public string StoreName { get; set; }

        public GeneralCommand<object> SetCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    parent.UserCode = this.UserCode;
                    parent.ComfirmCommand.Execute(null);
                });
            }
        }
    }
}
