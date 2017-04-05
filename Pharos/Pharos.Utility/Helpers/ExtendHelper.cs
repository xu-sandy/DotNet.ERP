using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
namespace Pharos.Utility.Helpers
{
    public static class ExtendHelper
    {
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null) return true;
            return string.IsNullOrWhiteSpace(obj.ToString());
        }
        public static void Each<T>(this IEnumerable<T> col, Func<T, bool> handler)
        {
            if (col == null) return;
            foreach (T local in col)
            {
                if (!handler(local))
                {
                    break;
                }
            }
        }
        public static void Each<T>(this IEnumerable<T> col, Action<T> handler)
        {
            if (col == null) return;
            foreach (T local in col)
            {
                handler(local);
            }
        }
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dicts, Dictionary<TKey, TValue> addDicts)
        {
            if (dicts == null || addDicts == null) return;
            foreach (var de in addDicts)
            {
                if (!dicts.ContainsKey(de.Key))
                    dicts.Add(de.Key, de.Value);
            }
        }
        /// <summary>
        /// Linq动态排序(反射.排序名称必须与Model一致)
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">要排序的数据源</param>
        /// <param name="value">排序依据（加空格）排序方式</param>
        /// <returns>IOrderedQueryable</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string fieldName, bool isAsc)
        {
            if (fieldName.IsNullOrEmpty()) return (IOrderedQueryable<T>)source;
            string Name = isAsc ? "OrderBy" : "OrderByDescending";
            return ApplyOrder<T>(source, fieldName, Name);
        }
        /// <summary>
        /// Linq动态排序再排序(反射.排序名称必须与Model一致)
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">要排序的数据源</param>
        /// <param name="value">排序依据（加空格）排序方式</param>
        /// <returns>IOrderedQueryable</returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string fieldName, bool isAsc)
        {
            string Name = isAsc ? "ThenBy" : "ThenByDescending";
            return ApplyOrder<T>(source, fieldName, Name);
        }
        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "a");
            PropertyInfo pi = type.GetProperty(property);
            Expression expr = Expression.Property(arg, pi);
            type = pi.PropertyType;
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            object result = typeof(Queryable).GetMethods().Single(
            a => a.Name == methodName
            && a.IsGenericMethodDefinition
            && a.GetGenericArguments().Length == 2
            && a.GetParameters().Length == 2).MakeGenericMethod(typeof(T), type).Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
        /// <summary>
        /// 去除重重
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector">p=>new { p.Id, p.Name }</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        /// <summary>
        /// 产生easyui自动分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="nvl">Request.Params</param>
        /// <returns></returns>
        public static List<TEntity> ToPageList<TEntity>(this IQueryable<TEntity> query, System.Collections.Specialized.NameValueCollection nvl = null)
        {
            nvl = nvl ?? HttpContext.Current.Request.Params;
            var pageIndex = 1;
            var pageSize = 0;
            var sort = "Id";
            var order = "asc";
            if (!nvl["page"].IsNullOrEmpty())
                pageIndex = int.Parse(nvl["page"]);
            if (!nvl["rows"].IsNullOrEmpty())
                pageSize = int.Parse(nvl["rows"]);
            if (!nvl["sort"].IsNullOrEmpty())
                sort = nvl["sort"];
            if (!nvl["order"].IsNullOrEmpty())
                order = nvl["order"];
            var st = sort;
            var or = order;
            string stthen = "",orthen="";
            if(sort.Contains(","))
            {
                st= sort.Split(',')[0];
                if(st.Contains(" "))
                {
                    or = st.Split(' ')[1];
                    st = st.Split(' ')[0];
                    if (or.IsNullOrEmpty())
                        or = "asc";
                }
                stthen = sort.Split(',')[1];
                orthen = order;
            }
            if (!stthen.IsNullOrEmpty())
                query = query.OrderBy(st, or == "asc").ThenBy(stthen, orthen == "asc");
            else
                query = query.OrderBy(st, or == "asc");

            if(pageSize>0)
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return query.ToList();
        }

        public static void ToCopyProperty<TEntity1, TEntity2>(this TEntity1 eny1, TEntity2 eny2, List<string> excludeProps = null) where TEntity1 : class
        {
            if (eny1 == null || eny2==null) return;
            var curType = eny1.GetType();
            var tarType = eny2.GetType();
            var propertyList = (from x in curType.GetProperties()
                                join y in tarType.GetProperties() on x.Name equals y.Name
                                select new
                                {
                                    cur = x,
                                    tar = y
                                });

            foreach (var p in propertyList)
            {
                try
                {
                    if (excludeProps != null && excludeProps.Contains(p.tar.Name)) continue;
                    if (!p.tar.CanWrite ||
                        (p.tar.PropertyType.IsGenericType && !p.tar.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))) continue;
                    var attrs = p.tar.GetCustomAttributes(typeof(Pharos.Utility.ExcludeAttribute), false);
                    if (attrs.Length > 0) continue;
                    p.tar.SetValue(eny2, p.cur.GetValue(eny1, null), null);
                }
                catch (Exception) { continue; }
            }
        }

        /// <summary>
        /// 对于两个相同类的属性进行复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void CopyProperty<T>(this T target, T source)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            Func<PropertyInfo, bool> filter = p => p.CanRead && p.CanWrite;
            var type = source.GetType();

            var sourceProperties = type.GetProperties(flags).Where(filter);
            var targetProperties = type.GetProperties(flags).Where(filter);

            foreach (var property in targetProperties)
            {
                var s = sourceProperties.SingleOrDefault(p => p.Name.Equals(property.Name)
                         && property.DeclaringType.IsAssignableFrom(p.DeclaringType));
                if (s != null)
                {
                    property.SetValue(target, s.GetValue(source, null), null);
                }
            }
        }
        /// <summary>
        /// 为空抛出异常
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="errMess"></param>
        public static TEntity IsNullThrow<TEntity>(this TEntity obj, string errMess = "传入参数不正确!") where TEntity : class
        {
            if (obj.IsNullOrEmpty()) throw new Exception(errMess);
            return obj;
        }
        /// <summary>
        /// DataTableTOJSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(this System.Data.DataTable dt)
        {
            System.Text.StringBuilder jsonBuilder = new System.Text.StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.AppendFormat("\"total\":{0}, ", dt.Rows.Count);
            jsonBuilder.Append("\"rows\":[ ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        /// <summary>
        /// JsonToXML
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonToXML(this string json)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                var xmlReader = System.Runtime.Serialization.Json.JsonReaderWriterFactory.CreateJsonReader(System.Text.Encoding.Default.GetBytes(json), System.Xml.XmlDictionaryReaderQuotas.Max);
                doc.Load(xmlReader);
                var declare = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
                doc.InsertBefore(declare, doc.DocumentElement);
            }
            catch { return ""; }
            return doc.OuterXml;
        }
        /// <summary>
        /// JsonToDataTable
        /// </summary>
        /// <param name="strJson">JSON</param>
        /// <returns></returns>
        public static System.Data.DataTable JsonToDataTable(this string strJson)
        {
            //转换json格式
            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            //创建表   
            if (tb == null && mc.Count > 0)
            {
                tb = new DataTable();
                tb.TableName = strName;
            }
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');
                foreach (string str in strRows)
                {
                    string[] strCell = str.Split('#');
                    string colName = "";
                    if (strCell[0].Substring(0, 1) == "\"")
                    {
                        int a = strCell[0].Length;
                        colName = strCell[0].Substring(1, a - 2);
                    }
                    else
                    {
                        colName = strCell[0];
                    }
                    if (!tb.Columns.Contains(colName))
                        tb.Columns.Add(colName);
                }
                //增加内容   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }
        /// <summary>
        /// 转成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            if (list == null || !list.Any()) return null;
            //PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            //var type = typeof(T);
            var type = list.FirstOrDefault().GetType();
            var properties = type.GetProperties();
            DataTable dt = new DataTable();
            for (int i = 0; i < properties.Count(); i++)
            {
                var property = properties[i];
                var type1 = property.PropertyType;
                if (type1.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    type1 = property.PropertyType.BaseType;
                dt.Columns.Add(property.Name, type1);
            }
            object[] values = new object[properties.Count()];
            foreach (T item in list)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
        public static void MoveTo(this DataColumnCollection dcs, string columnName, int index, ref DataTable dt)
        {
            if (dt == null || columnName.IsNullOrEmpty()) return;
            DataColumn resCol = null;
            for (var idx = 0; idx < dt.Columns.Count; idx++)
            {
                var col = dt.Columns[idx];
                if (string.Equals(col.ColumnName, columnName, StringComparison.CurrentCultureIgnoreCase))
                {
                    resCol = col;
                }
            }
            if (resCol == null) return;
            var data = new DataTable();
            for (var idx = 0; idx < dt.Columns.Count; idx++)
            {
                var col = dt.Columns[idx];
                if (idx == index)
                {
                    if (data.Columns.Contains(columnName))
                        data.Columns.Remove(columnName);
                    data.Columns.Add(resCol.ColumnName, resCol.DataType);
                    idx--;
                    index = -index;
                }
                else if (!data.Columns.Contains(col.ColumnName))
                    data.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataRow dr in dt.Rows)
            {
                var nr = data.NewRow();
                foreach (DataColumn col in data.Columns)
                {
                    nr[col.ColumnName] = dr[col.ColumnName];
                }
                data.Rows.Add(nr);
            }
            dt = data;
        }
        public static bool CloneTo(this DataColumn dc, string columnName)
        {
            var dt = dc.Table;
            if (dt == null || columnName.IsNullOrEmpty()) return false;
            if (dt.Columns.Contains(columnName)) return false;
            dt.Columns.Add(columnName);
            foreach (DataRow dr in dt.Rows)
            {
                dr[columnName] = dr[dc];
            }
            return true;
        }
        /// <summary>
        /// 对某列赋值，不存在不抛出异常信息
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        public static void SetValue(this DataRow row, string columnName, object value)
        {
            try
            {
                row[columnName] = value;
            }
            catch { };
        }
        /// <summary>
        /// 获取某列值，不存在不抛出异常信息
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column">index或name</param>
        /// <returns></returns>
        public static object GetValue(this DataRow row, dynamic column)
        {
            try
            {
                var obj = Convert.ToString(row[column]);
                if (!(obj as string).IsNullOrEmpty())
                    return obj;
            }
            catch { };
            return DBNull.Value;
        }
        public static T ToType<T>(this object obj)
        {
            try
            {
                var type = typeof(T);
                if (obj.IsNullOrEmpty())
                    return default(T);
                else if (!type.IsGenericType)
                    return (T)Convert.ChangeType(obj, type);
                else if (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    return (T)Convert.ChangeType(obj, Nullable.GetUnderlyingType(type));
                return default(T);
            }
            catch
            {
                return default(T);
            };

        }

        /// <summary>
        /// 自动转化整型
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static string ToAutoString(this decimal dec, int precision = 2)
        {
            var val = dec.ToString("f" + precision);
            var digit = val.Substring(val.IndexOf(".") + 1);
            if (int.Parse(digit) > 0)
            {
                while (true)
                {
                    if (!val.EndsWith("0")) break;
                    val = val.Remove(val.LastIndexOf("0"), 1);
                }
                return val;
            }
            return dec.ToString("f0");
        }
        /// <summary>
        /// 克隆列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> ToClone<T>(this IEnumerable<T> list) where T : class,new()
        {
            var ls = new List<T>();
            foreach (var t in list)
            {
                T obj = new T();
                t.ToCopyProperty(obj);
                ls.Add(obj);
            }
            return ls;
        }
        static object objLog = new object();
        public static void ToLog(this string str,string dir="")
        {
            lock (objLog)
            {
                if (!dir.IsNullOrEmpty()) dir += "\\";
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" +dir+ DateTime.Now.ToString("yyyy-MM")+"\\";
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                string filename = path + DateTime.Now.ToString("yyyyMMdd") + ".log";
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, true))
                {
                    sw.WriteLine(DateTime.Now.ToLocalTime().ToString());
                    sw.WriteLine(str);
                    sw.WriteLine("====================================================");
                    sw.Close();
                }
            }
        }
        /// <summary>
        /// 获取一个类指定的属性值
        /// </summary>
        /// <param name="info">object对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static object GetPropertyValue(this object info, string propertyName)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == propertyName.ToLower() select pi;
            return property.Any() ? property.First().GetValue(info, null) : null;
        }
        public static TValue GetValue<Tkey, TValue>(this Dictionary<Tkey, TValue> dicts, Tkey key)
        {
            if (dicts == null) return default(TValue);
            TValue val = default(TValue);
            if (typeof(Tkey) == typeof(string))
            {
                foreach (var d in dicts)
                {
                    if (string.Equals(d.Key.ToString(), key.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        val = d.Value;
                        break;
                    }
                }
            }
            else
                dicts.TryGetValue(key, out val);
            return val;
        }

        public static string GetEnumDescription(this Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }

        private static Random ran = new Random();
        /// <summary>
        /// 获取一个指定长度的随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>

        public static string GetRandomStr(int length, Func<string, bool> filter = null)
        {
        RandomToStr:
            Byte[] b = new Byte[length];
            ran.NextBytes(b);
            var securityCode = BitConverter.ToString(b).Replace("-", "").ToUpper();
            securityCode = securityCode.Substring(0, length);
            if (filter != null && filter(securityCode))
            {
                goto RandomToStr;
            }
            return securityCode;
        }

        public static Dictionary<string, string> ToDictionary(this System.Collections.Specialized.NameValueCollection nvl)
        {
            var dict = new Dictionary<string, string>();
            foreach (var key in nvl.AllKeys)
            {
                dict[key] = nvl[key];
            }
            return dict;
        }
        /// <summary>
        /// 获取虚拟目录（以斜杆结尾）
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetApplicationPath(this HttpContext context)
        {
            return context.Request.ApplicationPath == "/" ? "/" : context.Request.ApplicationPath + "/";
        }
        public static string TrimMore(this string str,short length)
        {
            if (!str.IsNullOrEmpty() && str.Length > length)
                return str.Substring(0, length)+"...";
            return str;
        }
        /// <summary>
        /// 把传参参数转成路由参数
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public static System.Web.Routing.RouteValueDictionary ToRouteValueDictionary(this System.Collections.Specialized.NameValueCollection nvc)
        {
            var dict = new System.Web.Routing.RouteValueDictionary();
            for(var i=0;i< nvc.Count;i++)
            {
                var key= nvc.GetKey(i);
                dict[key] = nvc[i];
            }
            return dict;
        }
        /// <summary>
        /// 逗号隔开转成整型数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this string str,char separator=',')
        {
            if (str.IsNullOrEmpty()) return new int[]{};
            return str.Split(separator).Where(o=>!o.IsNullOrEmpty()).Select(o=>int.Parse(o)).Distinct().ToArray();
        }
    }
}
