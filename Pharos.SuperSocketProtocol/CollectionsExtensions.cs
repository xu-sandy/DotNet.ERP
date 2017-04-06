using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.SuperSocketProtocol
{
    public static class CollectionsExtensions
    {
        /// <summary>
        /// 数组克隆拷贝
        /// </summary>
        /// <param name="source">拷贝源</param>
        /// <param name="offset">拷贝起点</param>
        /// <param name="length">拷贝长度</param>
        /// <returns>克隆拷贝数组</returns>
        public static T[] CloneRange<T>(this T[] source, int offset, int length)
        {
            var result = new T[length];
            Array.Copy(source, offset, result, 0, length);
            return result;
        }
    }
}
