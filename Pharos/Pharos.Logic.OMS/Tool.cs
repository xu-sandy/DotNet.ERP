using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.PinYinConverter;
using System.Collections.ObjectModel;
using System.IO;
using System.Web;

namespace Pharos.Logic.OMS
{
    public static class Tool
    {
        /// <summary>
        /// List转换 SelectListItem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="text">文本</param>
        /// <param name="value">值</param>
        /// <param name="firstTitle">第一个文本</param>
        /// <param name="emptyValue">第一个值</param>
        /// <param name="selectValue">选中值</param>
        /// <returns></returns>
        public static List<SelectListItem> CreateSelect<T>(this IList<T> t, string text, string value, string firstText = "", string firstValue = "", string selectValue="")
        {
            List<SelectListItem> l = new List<SelectListItem>();
            if (!String.IsNullOrEmpty(firstText))
            {
                l.Add(new SelectListItem { Text = firstText, Value = firstValue });
            }
            foreach (var item in t)
            {
                var propers = item.GetType().GetProperty(text);
                var valpropers = item.GetType().GetProperty(value);
                l.Add(new SelectListItem { Text = propers.GetValue(item, null).ToString(), Value = valpropers.GetValue(item, null).ToString(), Selected = valpropers.GetValue(item, null).ToString()==selectValue });
            }
            return l;
        }

        /// <summary>
        /// DataTable转换成List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(DataTable dt) where T : new()
        {

            // 定义集合
            List<T> ts = new List<T>();

            // 获得此模型的类型
            Type type = typeof(T);
            //定义一个临时变量
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量
                    //检查DataTable是否包含此列（列名==对象的属性名）  
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出
                        //取值
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                //对象添加到泛型集合中
                ts.Add(t);
            }

            return ts;

        }

        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="isDelfirst">删除列</param>
        /// <param name="dt"></param>
        /// <returns>多个列名“，”隔开</returns>
        public static List<string> getColumnName(DataTable dt, int[] delCol)
        {
            List<string> list = new List<string>();
            if (dt.Columns.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (delCol.Contains(i))
                    {
                        continue;
                    }
                    else
                    {
                        list.Add(dt.Columns[i].ColumnName);
                    }
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        #region  汉字转换拼音

        /// <summary>
        /// 汉字转换拼音（全拼）
        /// </summary>
        /// <param name="txt">汉字</param>
        /// <returns>拼音首个字母大写的全拼</returns>
        public static string ToPinYin(string txt)  
        {  
             txt = txt.Trim();  
             byte[] arr = new byte[2]; //每个汉字为2字节  
             StringBuilder result = new StringBuilder();//使用StringBuilder优化字符串连接              
             char[] arrChar = txt.ToCharArray();  
             foreach (char c in arrChar)  
             {  
                 arr = System.Text.Encoding.Default.GetBytes(c.ToString());//根据系统默认编码得到字节码  
                 if (arr.Length == 1)//如果只有1字节说明该字符不是汉字                  
                 {   result.Append(c.ToString());  
                     continue;  
                 }  
                 ChineseChar chineseChar = new ChineseChar(c);  
                 string p = chineseChar.Pinyins[0];
                 result.Append(p.Substring(0, 1).ToUpper());
                 result.Append(p.Substring(1, p.Length - 2).ToLower());  
             }  
             return result.ToString();  
         }

        #endregion

        /// <summary>
        /// 文件上传（图片）
        /// </summary>
        /// <param name="fu"></param>
        /// <param name="filepath">上传路径</param>
        /// <param name="fname">文件名称</param>
        /// <returns></returns>
        public static string[] fileUpload(HttpPostedFileBase fu, string filepath, string fname = "")
        {
            //提示信息和上传路径
            string[] intoFile = new string[2];
            //提示信息
            intoFile[0] = "";
            //上传路径
            intoFile[1] = "";
            string fn = "";
            if (!string.IsNullOrEmpty(fname))
            {
                fn = fname;
            }
            else
            {
                fn = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            }

            //if (fu.FileName == "")//判断是否上传了文件
            //{
            //    intoFile[0] = "请您选择您要上传的文件";
            //    return intoFile;
            //}
            string filetext = Path.GetExtension(fu.FileName).ToLower();
            //if (filetext != ".jpg" && filetext != ".jpeg" && filetext != ".gif" && filetext != ".png" && filetext != ".bmp")
            //{
            //    intoFile[0] = "上传的文件只能是*.jpg,*.jpeg,*.gif,*.png,*.bmp格式的";
            //    return intoFile;
            //}
            //if (fu.ContentLength > 1000 * 1024)
            //{
            //    intoFile[0] = "上传的文件必须小于1000KB";
            //    return intoFile;
            //}
            string filename = fn + filetext;
            string path = filepath;
            if (Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(path)) == false)
            {
                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(path));
            }
            fu.SaveAs(System.Web.HttpContext.Current.Server.MapPath(path + filename));
            //文件名
            intoFile[1] = filename;
            intoFile[0] = "文件上传成功";
            return intoFile;
        }

        #region 删除一个文件
        /// <summary>
        /// 删除一个文件
        /// </summary>
        /// <param name="">文件路径</param>
        /// <returns></returns>
        public static bool deleteFile(string filePath)
        {
            if (filePath != "")
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(filePath);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 验证文件
        /// </summary>
        /// <param name="fu"></param>
        /// <param name="KB">如：1000</param>
        /// <param name="Format">如：*.jpg,*.jpeg,*.gif,*.png,*.bmp</param>
        /// <returns></returns>
        public static string ValidateFile(HttpPostedFileBase fu, int KB, string Format)
        {
            if (!string.IsNullOrEmpty(fu.FileName))
            {
                string filetext = Path.GetExtension(fu.FileName).ToLower();
                if (!Format.Contains(filetext))
                {
                    return "上传的文件只能是" + Format + "格式的";
                }
                if (fu.ContentLength > KB * 1024)
                {
                    return "上传的文件必须小于"+KB+"KB";
                }
            }
            return "success";
        }

        /// <summary>
        /// 获取代理商档案的证件照路径
        /// </summary>
        /// <param name="agentsId"></param>
        /// <returns></returns>
        public static string getIdCardPhotoPath(int agentsId)
        {
            return "/SysImg/" + agentsId + "/IdCardPhoto/";
        }

        /// <summary>
        /// 获取支付许可图片路径
        /// </summary>
        /// <param name="LicenseId">支付许可编号</param>
        /// <param name="type">1是资质或证；2是企业证件；3是身份证正面；4是身份证反面</param>
        /// <returns></returns>
        public static string getPLicensePicPath(string LicenseId, int type)
        {
            return "/SysImg/PayLicense/" + LicenseId + "/pic" + type + "/";
        }

        /// <summary> 
        /// 判断给定的字符串(strNumber)是否是数值型 
        /// </summary> 
        /// <param name="strNumber">要确认的字符串</param> 
        /// <returns>是则返加true 不是则返回 false</returns> 
        public static bool IsNumber(string strNumber)
        {
            return new Regex(@"^([0-9])[0-9]*(\.\w*)?$").IsMatch(strNumber);
        }
    }
}
