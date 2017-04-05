using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Pharos.Utility
{
    /// <summary>
    /// 排除自动赋值属性
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ExcludeAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string positionalString;
        public ExcludeAttribute()
        {
        }
        // This is a positional argument
        public ExcludeAttribute(string positionalString)
        {
            this.positionalString = positionalString;
            // TODO: Implement code here
        }

        public string PositionalString
        {
            get { return positionalString; }
        }

        // This is a named argument
        public int NamedInt { get; set; }
    }


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class ExcelAttribute : Attribute
    {
        private string _columnName;

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        private int _columnIndex;

        public int ColumnIndex
        {
            get { return _columnIndex; }
            set { _columnIndex = value; }
        }

        public string Title { get; set; }

        public ExcelAttribute(string columnName, int columnIndex)
        {
            this._columnName = columnName;
            this._columnIndex = columnIndex;
        }

        public ExcelAttribute(string title)
        {
            Title = title;
        }
    }

    public class ExcelFieldAttribute : Attribute
    {
        public ExcelFieldAttribute(string rules)
        {
            try
            {

                var str = new string[1];
                str[0] = "@@@";
                var str2 = new string[1] { "###" };
                Rules = rules.Split(str, StringSplitOptions.RemoveEmptyEntries).Select(o => o.Split(str2, StringSplitOptions.None)).ToDictionary(o => o[0], o => o[1]);
            }
            catch 
            {
                
            }
        }
        public Dictionary<string, string> Rules { get; set; }

        public VerifyFiledResult VerifyFiled(object value)
        {
            foreach (var item in Rules)
            {
                if (!Regex.IsMatch(value.ToString(), item.Key))
                {
                    return new VerifyFiledResult() { IsSuccess = false, Message = item.Value };
                }
            }
            return new VerifyFiledResult() { IsSuccess = true };
        }
    }

    public class LocalKeyAttribute : Attribute
    {

    }

}
