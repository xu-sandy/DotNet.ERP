using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Pharos.Logic.Entity
{
    //MembershipCardSetting
    public class MembershipCardSetting
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 公司CID
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 磁卡前缀字符
        /// </summary>
        public string StartChar { get; set; }
        /// <summary>
        /// 磁卡后缀字符
        /// </summary>
        public string EndChar { get; set; }
        /// <summary>
        /// 射频卡使用扇区（1-15）
        /// </summary>
        public short BootSector { get; set; }
        /// <summary>
        /// 验证防伪码（0：不验证；1：验证）
        /// </summary>
        public bool CheckSecuritycode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUID { get; set; }

    }
}