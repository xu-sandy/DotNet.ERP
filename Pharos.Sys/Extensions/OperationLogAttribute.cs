using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Sys.Extensions
{
    public class OperationLogAttribute : Attribute
    {
        /// <summary>
        /// 枚举或者自定义字典,且已知字典显示项属性
        /// </summary>
        /// <param name="title"></param>
        /// <param name="prop"></param>
        public OperationLogAttribute(string title, string prop)
        {
            IsDict = true;
            Source = DictSource.Prop;
            Title = title;
            Prop = prop;
        }
        /// <summary>
        /// 枚举或者自定义字典
        /// </summary>
        /// <param name="title"></param>
        /// <param name="dicts"></param>
        public OperationLogAttribute(string title, params string[] dicts)
        {
            IsDict = true;
            Title = title;
            ShowDicts = dicts;
            Source = DictSource.Enum;
        }
        /// <summary>
        /// 标记字典（字典必须在数据库字典表中）或普通属性
        /// </summary>
        /// <param name="title"></param>
        /// <param name="isDict"></param>
        public OperationLogAttribute(string title, bool isDict)
        {
            IsDict = isDict;
            if (isDict)
            {
                Source = DictSource.InDBSysDict;
            }
            Title = title;
        }
        public bool IsDict { get; set; }

        public DictSource Source { get; set; }

        public string Prop { get; set; }

        public string[] ShowDicts { get; set; }

        public string Title { get; set; }
    }

    public enum DictSource
    {
        InDBSysDict = 0,
        Enum = 1,
        Prop = 2
    }
}
