// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：ywb
// 创建时间：2015-07-16
// 描述信息：系统菜单信息Model
// --------------------------------------------------

using Newtonsoft.Json;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharos.Logic.EntityExtend
{
	/// <summary>
	/// 菜单信息
	/// </summary>
    [NotMapped]
	[Serializable]
	public class SysMenusExt:SysMenus
	{
        public SysMenusExt() { }
        public SysMenusExt(SysMenus model)
        {
            Id = model.Id;
            PId = model.PId;
            Title = model.Title;
            Status = model.Status;
            LimitsCode = model.LimitsCode;
            SortOrder = model.SortOrder;
            URL = model.URL;
        }

        /// <summary>
        /// 父级菜单名称
        /// </summary>
        public string PTitle { get; set; }

        /// <summary>
        /// 子级菜单集合
        /// </summary>
        [JsonProperty("children")]
        public List<SysMenusExt> Childs { get; set; }
    }
}
