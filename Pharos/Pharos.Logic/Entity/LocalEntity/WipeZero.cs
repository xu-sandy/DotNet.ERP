using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Logic.LocalEntity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.LocalEntity
{
    public class WipeZero : BaseEntity, ICanUploadEntity
    {
        public Int64 Id { get; set; }
        /// <summary>
        /// 支付编号
        /// </summary>
        [LocalKey]
        
        public string PaySN { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Number { get; set; }

        public bool IsUpload { get; set; }

        public DateTime CreateDT { get; set; }
    }
}
