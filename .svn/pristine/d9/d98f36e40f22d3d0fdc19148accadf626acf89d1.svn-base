using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class SysMailSender:BaseEntity
    {
        public string Id { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string BatchNo { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        /// <summary>
        ///状态 0-草稿1-已发送
        /// </summary>
        public short State { get; set; }
        /// <summary>
        /// 发件人
        /// </summary>
        [Pharos.Utility.Exclude]
        public string SenderCode { get; set; }
        [Pharos.Utility.Exclude]
        public string SenderName { get; set; }
        /// <summary>
        /// 发件时间
        /// </summary>
        [Pharos.Utility.Exclude]
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 定时时间
        /// </summary>
        public DateTime? TimerDate { get; set; }
        public virtual List<Attachment> Attachments { get; set; }
        /// <summary>
        /// 接收者:姓名(编码),
        /// </summary>
        public virtual string ReceiverCodes { get; set; }
        public virtual string CopytoCodes { get; set; }
    }
}
