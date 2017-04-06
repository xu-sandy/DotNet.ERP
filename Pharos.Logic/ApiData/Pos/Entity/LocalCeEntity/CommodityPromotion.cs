using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public partial class CommodityPromotion
    {

        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：19]
        /// [不允许为空]
        /// </summary>
        [Key]
        public string Id { get; set; }
        /// <summary>
        /// 企业标识
        /// </summary>
        public int CompanyId { get; set; }
        public string StoreId { get; set; }
        public short CustomerObj { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short Timeliness { get; set; }
        public string StartAging1 { get; set; }
        public string EndAging1 { get; set; }
        public string StartAging2 { get; set; }
        public string EndAging2 { get; set; }
        public string StartAging3 { get; set; }
        public string EndAging3 { get; set; }
        public short PromotionType { get; set; }
        public short RestrictionBuyNum { get; set; }
        public short State { get; set; }
        public DateTime CreateDT { get; set; }
        public string CreateUID { get; set; }
        [Required]
        public Guid SyncItemId { get; set; }
        [MaxLength(8)]
        [Required]
        public byte[] SyncItemVersion { get; set; }
    }
}
