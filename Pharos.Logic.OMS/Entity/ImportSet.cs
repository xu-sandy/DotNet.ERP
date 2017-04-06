using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
namespace Pharos.Logic.OMS.Entity
{
    /// <summary>
    /// 基础表导入配置
    /// </summary>
    public class ImportSet
    {
        public int Id { get; set; }
        /// <summary>
        /// 导入表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 字段名称和对应Excel列列
        /// </summary>
        public string FieldJson { get; set; }
        /// <summary>
        /// 起始行数
        /// </summary>
        public short MinRow { get; set; }
        /// <summary>
        /// 最大行数
        /// </summary>
        public short? MaxRow { get; set; }
        /// <summary>
        /// 相关联不存在是否创建新记录，0－不创建，1－创建
        /// </summary>
        public bool RefCreate { get { return _RefCreate; } set { _RefCreate = value; } }
        bool _RefCreate = true;
        public Dictionary<string,string> FieldCols
        {
            get
            {
                if (FieldJson.IsNullOrEmpty()) return null;
                var list= FieldJson.ToObject<List<KeyValuePair<string, string>>>();
                if (list == null || !list.Any()) return null;
                return list.ToDictionary(o => o.Key, o => o.Value);
            }
        }
    }
}
