﻿using System.Collections.Generic;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T">分页数据类型</typeparam>
    public class PageResult<T> : IPageResult
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

    public interface IPageResult 
    {
        PageInfo Pager { get; set; }
    }
    public class PageInfo
    {
        public int Total { get; set; }

        public int PageCount { get; set; }

        public int Index { get; set; }

        public int Size { get; set; }
    }
}
