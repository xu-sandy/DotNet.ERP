using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Utility.Helpers
{
    public class StringHelper
    {
        #region 获取后几位数 public static string GetLastStr(string str,int num)
        /// <summary>
        /// 获取后几位数
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="num">返回的具体位数</param>
        /// <returns>返回结果的字符串</returns>
        public static string GetLastStr(string str, int num)
        {
            int count = 0;
            if (str.Length > num)
            {
                count = str.Length - num;
                str = str.Substring(count, num);
            }
            return str;
        }
        #endregion
    }
}
