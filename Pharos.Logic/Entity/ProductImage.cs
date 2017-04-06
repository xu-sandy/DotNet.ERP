using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class ProductImage:BaseEntity
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        /// <summary>
        /// 图片位置
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// 状态  1-当前使用
        /// </summary>
        public bool Status { get; set; }
        public DateTime? CreateDT { get; set; }
        public string CreateUID { get; set; }
    }
}
