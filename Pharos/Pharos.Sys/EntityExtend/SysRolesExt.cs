// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：ywb
// 创建时间：2015-07-16
// 描述信息：系统菜单信息Model
// --------------------------------------------------

using Pharos.Sys.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pharos.Sys.EntityExtend
{
    [NotMapped]
    [Serializable]
    [DataContract(IsReference = true)]
    public class SysRolesExt : SysRoles
    {

    }
}
