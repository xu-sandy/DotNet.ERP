﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.Entity.LocalCeEntity
{
    public class Area
    {
        [Key]
        public int ID { get; set; }
        public int AreaID { get; set; }
        public int AreaPID { get; set; }
        public string Title { get; set; }
        public byte Type { get; set; }
        public string JianPin { get; set; }
        public string QuanPin { get; set; }
        public string AreaSN { get; set; }
        public string PostCode { get; set; }
        public int OrderNum { get; set; }
        [Required]
        public Guid SyncItemId { get; set; }
        [MaxLength(8)]
        [Required]
        public byte[] SyncItemVersion { get; set; }
    }
}
