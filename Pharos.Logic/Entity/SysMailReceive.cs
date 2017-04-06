using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class SysMailReceive : BaseEntity
    {
        public string Id { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 接收者
        /// </summary>
        public string ReceiveCode { get; set; }
        public string ReceiveName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        /// <summary>
        /// 1-收件人2-抄送人
        /// </summary>
        public short Type { get; set; }
        /// <summary>
        /// 0-未读1-已读
        /// </summary>
        public short State { get; set; }
        /// <summary>
        /// 发件人
        /// </summary>
        public string SenderCode { get; set; }
        public string SenderName { get; set; }
        /// <summary>
        /// 发件时间
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 已读时间
        /// </summary>
        public DateTime? ReadDate { get; set; }
        public virtual List<Attachment> Attachments { get; set; }
        /// <summary>
        /// 接收者:姓名(编码),
        /// </summary>
        public virtual string ReceiverCodes { get; set; }
        public virtual string CopytoCodes { get; set; }
    }
}
