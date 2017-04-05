using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pharos.Utility.Helpers
{
    public class EnumHelper
    {
        private static Hashtable _htb = new Hashtable();
        private static Hashtable _htbAll = new Hashtable();
        public static Type AttributeType = typeof(DescriptionAttribute);

        /// <summary>
        /// 根据枚举类型获得枚举中文描述以及枚举属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<DropdownItem> GetList<T>()
        {
            return GetList(typeof(T));
        }
        /// <summary>
        /// 根据枚举类型获得枚举中文描述以及枚举属性值，是否添加全部Item项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isAll">是否有全部选项</param>
        /// <returns></returns>
        public static List<DropdownItem> GetList<T>(bool isAll)
        {
            return GetList(typeof(T), isAll);
        }
        /// <summary>
        /// 根据枚举类型获得枚举中文描述以及枚举属性值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isAll">是否有全部选项</param>
        /// <returns></returns>
        public static List<DropdownItem> GetList(Type type, bool isAll)
        {
            var result = GetList(type);
            if (isAll)
            {
                result.Insert(0, new DropdownItem() { Value = "", Text = "全部" });
            }
            return result;
        }
        /// <summary>
        /// 根据枚举类型获得枚举中文描述以及枚举属性值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<DropdownItem> GetList(Type type)
        {
            FieldInfo[] fields = type.GetFields();
            var enumNames = type.GetEnumNames();
            List<DropdownItem> list = new List<DropdownItem>();
            for (int i = 0; i < enumNames.Length; i++)
            {
                FieldInfo item = type.GetField(enumNames[i]);
                object[] objs = item.GetCustomAttributes(AttributeType, false);
                string desription;
                if (objs != null && objs.Length != 0)
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    if (string.IsNullOrEmpty(da.Description))
                        continue;
                    desription = da.Description;
                }
                else
                {
                    desription = item.Name;
                }
                int labelID = -1;
                try
                {
                    labelID = Convert.ToInt32(System.Enum.Parse(type, item.Name));
                }
                catch (Exception ex)
                {
                    //we will ignore this at the moment in case of any invalid value from database.
                    desription = "[Unkown Error] " + item.Name + " " + ex.Message;
                }
                list.Add(new DropdownItem() { Text = desription, Value = labelID.ToString() });
            }
            return list;
        }
        /// <summary>
        /// 根据枚举类型获得枚举中文描述以及枚举属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<DropdownItem> GetCacheList<T>()
        {
            return GetCacheList(typeof(T));
        }
        /// <summary>
        /// 根据枚举类型获得枚举中文描述以及枚举属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isAll">是否有全部选项</param>
        /// <returns></returns>
        public static List<DropdownItem> GetCacheList<T>(bool isAll)
        {
            return GetCacheList(typeof(T), isAll);
        }
        /// <summary>
        /// 根据枚举类型获得枚举中文描述以及枚举属性值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<DropdownItem> GetCacheList(Type type)
        {
            return GetCacheList(type, false);
        }
        /// <summary>
        /// 根据枚举类型获得枚举中文描述以及枚举属性值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isAll">是否有全部选项</param>
        /// <returns></returns>
        public static List<DropdownItem> GetCacheList(Type type, bool isAll)
        {
            if (isAll)
            {
                return GetCacheList(_htbAll, type, isAll);
            }
            else
            {
                return GetCacheList(_htb, type, isAll);
            }
        }
        private static List<DropdownItem> GetCacheList(Hashtable htb, Type type, bool isAll)
        {
            if (htb.Contains(type))
                return htb[type] as List<DropdownItem>;
            else
            {
                var result = GetList(type, isAll);
                htb[type] = result;
                return result;
            }
        }

        /// <summary>
        /// 将枚举的描述文件以字符串方式输出
        /// </summary>
        /// <param name="subenum"></param>
        /// <returns></returns>
        public static string GetDescription(System.Enum subenum)
        {
            string description = subenum.ToString();
            FieldInfo fieldInfo = subenum.GetType().GetField(subenum.ToString());
            object[] objs = fieldInfo.GetCustomAttributes(AttributeType, false);
            if (objs != null && objs.Length != 0)
            {
                DescriptionAttribute da = (DescriptionAttribute)objs[0];
                description = da.Description;
            }
            return description;
        }


    }
}
