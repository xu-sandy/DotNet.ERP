using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public class BaseEntity
    {
        /// <summary>
        /// 记录ID
        /// [主键：√]
        /// [长度：19]
        /// [不允许为空]
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        /// <summary>
        /// 企业标识
        /// </summary>
        public int CompanyId { get; set; }
        [Required]
        public Guid SyncItemId { get; set; }
        [MaxLength(8)]
        [Required]
        public byte[] SyncItemVersion { get; set; }
    }
}
