﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.Entity
{
    public partial class Realm
    {
        public int Id { get; set; }
        /// <summary>
        /// -1-保留域名
        /// </summary>
        public int CID { get; set; }
        /// <summary>
        /// 域名前缀
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 一级域名（来自字典）
        /// </summary>
        public int? Domain1 { get; set; }
        /// <summary>
        /// 可用状态(1-可用,0-停用,-1-未审核)
        /// </summary>
        public short State { get; set; }
        /// <summary>
        /// 类别（1是内部，2是外部）
        /// </summary>
        public short? Category { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDT { get; set; }
    }
}
