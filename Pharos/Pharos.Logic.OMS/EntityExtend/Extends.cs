﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
namespace Pharos.Logic.OMS.Entity
{
    public partial class ProductCategory
    {
        [NotMapped]
        [JsonProperty("children")]
        public virtual List<ProductCategory> Childrens { get; set; }
        /// <summary>
        /// 树形涨开或收缩(open|closed)
        /// </summary>
        [NotMapped]
        [JsonProperty("state")]
        public string OnOff { get; set; }
        [NotMapped]
        public string CategoryPSNTitle { get; set; }
        [NotMapped]
        public string StateTitle { get { return Enum.GetName(typeof(EnableState), State); } }
        [NotMapped]
        public bool IsRemove { get; set; }
        [NotMapped]
        public int ProductCount { get; set; }
        [NotMapped]
        [JsonProperty("text")]
        public string Text { get { return (CategoryCode > 0 ? "[" + CategoryCode.ToString("00") + "]" : "") + Title; } }
        [JsonProperty("id")]
        [NotMapped]
        public int SN { get { return CategorySN; } }
    }
    public partial class VwProduct:BaseProduct
    {
        public string CategoryTitle { get; set; }
        public string SubUnit { get; set; }
        public string BrandTitle { get; set; }
        [NotMapped]
        public string BrandClassTitle { get; set; }
        public string StateTitle { get { return State == 0 ? "已下架" : "已上架"; } }
        /// <summary>
        /// 商家量
        /// [长度：5]
        /// </summary>
        public int TraderNum { get; set; }
    }

    public class SysDataDictionaryExt : BaseDataDictionary
    {
        public SysDataDictionaryExt() { }
        /// <summary>
        /// 显示的子项字符串
        /// </summary>
        public string ItemsStr { get; set; }
        /// <summary>
        /// 所有子项树
        /// </summary>
        public int ItemsCount { get; set; }
        /// <summary>
        /// 父级字典名称
        /// </summary>
        public string PTitle { get; set; }
    }

    public partial class Area
    {
        [NotMapped]
        [JsonProperty("children")]
        public virtual List<Area> Childrens { get; set; }
        /// <summary>
        /// 树形涨开或收缩(open|closed)
        /// </summary>
        [NotMapped]
        [JsonProperty("state")]
        public string OnOff { get; set; }
        [NotMapped]
        public string PTitle { get; set; }
        [NotMapped]
        public int TraderNum { get; set; }
        [NotMapped]
        [JsonProperty("text")]
        public string Text { get { return Title; } }
        [NotMapped]
        public int Id { get { return AreaID; } }
    }
    public partial class DeviceAuthorize
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        [NotMapped]
        public string DeviceName { get; set; }
    }
    public partial class CompanyAuthorize
    {
        [JsonIgnore]
        [NotMapped]
        public string RealmName { get; set; }
        [JsonIgnore]
        [NotMapped]
        public string RealmUrl { get; set; }
        [JsonIgnore]
        [NotMapped]
        public string RealmSuffixUrl { get; set; }
        [JsonIgnore]
        [NotMapped]
        public bool RealmState { get { return _RealmState; } set { _RealmState = value; } }
        bool _RealmState = true;
    }
    public partial class Attachment
    {
        [NotMapped]
        public string FileSizeTitle { get { return Pharos.Utility.Helpers.FileHelper.GetFileSize(FileSize); } }
        [NotMapped]
        public string FileFontIcon { get { return Pharos.Utility.Helpers.FileHelper.GetFileFontIcon(ExtName); } }
        [NotMapped]
        public byte[] Bytes { get; set; }
    }
    public partial class ProductModuleVer
    {
        public virtual List<ProductMenuLimit> ProductMenuLimits { get; set; }
        public virtual string StatusTitle { get { return Status == 1 ? "已生效" : Status == 2 ? "已失效" : "未生效"; } }
        public virtual string VerStatusTitle { get { return VerStatus == 1 ? "测试版" : VerStatus == 2 ? "正式版" : "未发布"; } }
    }
    public partial class ProductRoleVer
    {
        public virtual List<ProductRole> ProductRoles { get; set; }
        public virtual string StatusTitle { get { return Status == 1 ? "已生效" : Status == 2 ? "已失效" : "未生效"; } }
        public virtual string VerStatusTitle { get { return VerStatus == 1 ? "测试版" : VerStatus == 2 ? "正式版" : "未发布"; } }
    }
    public partial class ProductRole
    {
        public virtual List<ProductRoleData> ProductRoleDatas { get; set; }
    }
    public partial class ProductDictionaryVer
    {
        public virtual List<ProductDictionaryData> ProductDictionaryDatas { get; set; }
        public virtual string StatusTitle { get { return Status == 1 ? "已生效" : Status == 2 ? "已失效" : "未生效"; } }
        public virtual string VerStatusTitle { get { return VerStatus == 1 ? "测试版" : VerStatus == 2 ? "正式版" : "未发布"; } }
    }
    public partial class ProductDataVer
    {
        public virtual List<ProductDataSql> ProductDataSqls { get; set; }
        public virtual string StatusTitle { get { return Status == 1 ? "已生效" : Status == 2 ? "已失效" : "未生效"; } }
        public virtual string VerStatusTitle { get { return VerStatus == 1 ? "测试版" : VerStatus == 2 ? "正式版" : "未发布"; } }
    }
    public partial class ProductPublishVer
    {
        public virtual List<ProductUpdateLog> ProductUpdateLogs { get; set; }
        public virtual List<ProductPublishSql> ProductPublishSqls { get; set; }
        public virtual string StatusTitle { get { return Status == 1 ? "已生效" : Status == 2 ? "已失效" : "未生效"; } }
        public virtual string VerStatusTitle { get { return VerStatus == 1 ? "测试版" : VerStatus == 2 ? "正式版" : "未发布"; } }
        public virtual string VerCodeTitle { get { return VerCode == 0 ? "--" : "v"+VerCode.ToString("f1"); } }
        public virtual ProductModuleVer ProductModuleVer { get; set; }
        public virtual ProductRoleVer ProductRoleVer { get; set; }
        public virtual ProductDictionaryVer ProductDictionaryVer { get; set; }
        public virtual ProductDataVer ProductDataVer { get; set; }
        [NotMapped]
        public virtual string HasModelId { get; set; }
        [NotMapped]
        public virtual string HasRoleId { get; set; }
        [NotMapped]
        public virtual string HasDictId { get; set; }
        [NotMapped]
        public virtual string HasDataId { get; set; }
    }
    public partial class SysRoles
    {
        public virtual List<SysRoleData> SysRoleDatas { get; set; }
    }
    public partial class Business
    {
        [NotMapped]
        [JsonProperty("children")]
        public virtual List<Business> Childrens { get; set; }
        [NotMapped]
        public int TraderNum { get; set; }
        [NotMapped]
        public int PayLicenseNum { get; set; }
    }
    [NotMapped]
    public class DepartMentExt:SysDepartments
    {
        [JsonProperty("children")]
        public virtual List<DepartMentExt> Childrens { get; set; }
        public int ParentId { get; set; }
        /// <summary>
        /// 树形涨开或收缩(open|closed)
        /// </summary>
        [JsonProperty("state")]
        public string TreeState { get; set; }
        [JsonProperty("text")]
        public string TreeText { get { return Title; } }
        [JsonProperty("id")]
        public int TreeId { get { return DeptId; } }
        public int Index { get; set; }
        public int PrevId { get; set; }
        public int UserCount { get; set; }
        public int PositionCount { get; set; }
        public string ManagerTitle { get; set; }
        public string DeputyTitle { get; set; }

        public string FullTitle { get; set; }
    }

    public partial class Realm
    {
        /// <summary>
        /// 客户全称
        /// </summary>
        [NotMapped]
        public string TradersFullTitle { get; set; }
    }
}
