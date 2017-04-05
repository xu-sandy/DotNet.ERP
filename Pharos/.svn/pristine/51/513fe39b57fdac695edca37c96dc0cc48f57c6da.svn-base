using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class UserInfo
    {
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
        /// 登录密钥
        /// </summary>
        [JsonIgnore]
        public string LoginPwd { get; set; }

        /// <summary>
        /// 性别（ 0:女、 1:男）
        /// </summary>
        public bool Sex { get; set; }

        /// <summary>
        /// 本店角色（ 1:店长、 2:营业员、 3:仓管员）（多个 ID以,号间隔）
        /// </summary>
        [JsonIgnore]
        public string OperateAuth { get; set; }


        /// <summary>
        /// 最近登录时间
        /// </summary>
        public DateTime LoginDT { get; set; }
        /// <summary>
        /// 门店名称（即出货仓）
        /// </summary>
        public string StoreName { get; set; }
    }
}
