using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T">分页数据类型</typeparam>
    public class PageResult<T>
    {
        /// <summary>
        /// 分页数据内容
        /// </summary>
        public IEnumerable<T> Datas { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        public PageInfo Pager { get; set; }
    }
}
