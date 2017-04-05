/*----------------------------------------------------------------
// 功能描述：常规算法或运算
 * 创 建 人：蔡少发
// 创建时间：2015-04-23
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace Pharos.Utility
{
    /// <summary>
    /// 常规算法或运算
    /// </summary>
    public class Arithmetic
    {
        #region 两数相除得数 进一法 [常用于分页]

        /// <summary>
        /// 两数相除得数 进一法 [常用于分页]
        /// </summary>
        /// <param name="TotalRecord">总记录数</param>
        /// <param name="PageSize">每页显示N行</param>
        /// <returns>返回总有X页</returns>
        public static int GetCeiling(double TotalRecord, double PageSize)
        {
            return (int)Math.Ceiling(TotalRecord / PageSize);
        }

        #endregion

        #region 随机产生一组指定长度的不重复数

        /// <summary>
        /// 随机产生一组指定长度的不重复数
        /// </summary>
        /// <param name="length">指定长度</param>
        /// <returns>返回一组不重复数</returns>
        public static string GetRandom(byte length)
        {
            string str = string.Empty;
            Random rnd = new Random();
            int num = 0;

            while (str.Length < length)
            {
                num = rnd.Next(1, length + 1);

                if (!str.Contains(num.ToString()))
                {
                    str += num.ToString();
                }
            }

            return str;
        }

        #endregion

        #region 根据提供的字符随机产生 N 组指定长度的不重复数

        /// <summary>
        /// 根据提供的字符随机产生 N 组指定长度的不重复数
        /// </summary>
        /// <param name="strSource">自定义的字符源</param>
        /// <param name="length">指定长度</param>
        /// <param name="group">需产生 N 组</param>
        /// <returns>返回不重复数组</returns>
        public static List<string> GetRandom(string strSource, byte length, byte group)
        {
            if (string.IsNullOrEmpty(strSource)) { strSource = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; }

            List<string> myList = new List<string>();
            Random rnd = new Random();

            string first, str;
            int num = 0;

            while (myList.Count < group)
            {
                first = string.Empty;
                str = string.Empty;

                while (str.Length < length)
                {
                    num = rnd.Next(1, strSource.Length);

                    first = strSource.Substring(num, 1);

                    if (!str.Contains(first))
                    {
                        str += first;
                    }
                }

                if (!myList.Contains(str))
                {
                    myList.Add(str);
                }
            }

            return myList;
        }

        #endregion

        #region int[]排序
        //todo:

        #endregion
    }
}
