// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-22
// 描述信息：用于管理本系统的所有产品档案基本信息
// --------------------------------------------------

using System;
using System.Runtime.Serialization;
using Pharos.Utility;

namespace Pharos.Logic.Entity
{
    /// <summary>
    /// 产品档案信息表
    /// </summary>
    [Serializable]

    [Excel("商品信息")]
    public partial class ProductRecord : BaseProduct
    {
    }
}
