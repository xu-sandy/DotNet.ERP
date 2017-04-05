using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pharos.Utility
{
    /// <summary>
    /// EF 分页方式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PageList<T>:List<T>
    {
        #region EF 分页方式

        /// <summary>
        /// 不分页
        /// </summary>
        /// <param name="source"></param>
        public PageList(IQueryable<T> source)
        {
            var result = source.ToList();
            this.PageSetting(1, result.Count, source.Count());
            this.AddRange(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        public PageList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            this.PageSetting(pageIndex, pageSize, source.Count());
            this.AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        public PageList(IList<T> source, int pageIndex, int pageSize)
        {
            this.PageSetting(pageIndex, pageSize,source.Count());
            this.AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        #endregion

        #region 分页计算

        private void PageSetting(int pageIndex,int pageSize,int totalCount)
        {
            this.TotalCount = totalCount;
            this.TotalPages = this.TotalCount / pageSize;

            if (this.TotalCount % pageSize > 0)
                this.TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }

        /// <summary>
        /// page index
        /// </summary>
        public int PageIndex { get; private set; }
        /// <summary>
        /// page size
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// page row count
        /// </summary>
        public int TotalCount { get; private set; }
        /// <summary>
        /// total page
        /// </summary>
        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex < TotalPages); }
        }

        #endregion
    }

    /// <summary>
    /// 分页属性（适用于特定DB存储过程分页）
    /// </summary>
    public class Paging
    {
        /// <summary>
        /// 设置分页参数（由DBFramework调用）
        /// </summary>
        /// <param name="pageIndex">当前页索引号</param>
        /// <param name="pageSize">每页多少条</param>
        /// <param name="recordTotal">记录总条数</param>
        /// <param name="recordStart">当前页起始记录号</param>
        /// <param name="recordEnd">当前页结束记录号</param>
        public void SetPaging(int pageIndex, int pageSize, int recordTotal, int recordStart, int recordEnd)
        {
            PageSize = pageSize;
            RecordTotal = recordTotal;
            PageCount = (int)Math.Ceiling((double)recordTotal / pageSize);
            RecordStart = recordStart;
            RecordEnd = recordEnd;
            PageIndex = pageIndex > PageCount ? PageCount : pageIndex;
        }

        #region 必选，用户传参

        /// <summary>
        /// 当前页索引号
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页多少条
        /// </summary>
        public int PageSize { get; set; }

        #endregion

        #region 只读，内部计算

        /// <summary>
        /// 记录总条数
        /// </summary>
        public int RecordTotal { get; private set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// 当前页起始记录号
        /// </summary>
        public int RecordStart { get; private set; }

        /// <summary>
        /// 当前页结束记录号
        /// </summary>
        public int RecordEnd { get; private set; }

        /// <summary>
        /// 是否存在上一页
        /// </summary>
        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }
        /// <summary>
        /// 是否存在下一页
        /// </summary>
        public bool HasNextPage
        {
            get { return (PageIndex < PageCount); }
        }

        #endregion
    }

    /// <summary>
    /// 分页属性（适用于特定DB存储过程分页）
    /// </summary>
    public class Paging<T> : List<T>
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public Paging(DataTable dt, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;

            int recordTotal = 0;
            int recordStart = 0;
            int recordEnd = 0;

            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("RecordTotal"))
            {
                recordTotal = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                dt.Columns.Remove("RecordTotal");

                if (dt.Columns.Contains("RecordStart"))
                {
                    recordStart = Convert.ToInt32(dt.Rows[0]["RecordStart"]);
                    dt.Columns.Remove("RecordStart");
                }
                if (dt.Columns.Contains("RecordEnd"))
                {
                    recordEnd = Convert.ToInt32(dt.Rows[0]["RecordEnd"]);
                    dt.Columns.Remove("RecordEnd");
                }
            }

            SetPaging(PageIndex, PageSize, recordTotal, recordStart, recordEnd);
        }

        #region 由 DBHelper 调用处理

        /// <summary>
        /// 设置分页参数（由DBFramework调用）
        /// </summary>
        /// <param name="pageIndex">当前页索引号</param>
        /// <param name="pageSize">每页多少条</param>
        /// <param name="recordTotal">记录总条数</param>
        /// <param name="recordStart">当前页起始记录号</param>
        /// <param name="recordEnd">当前页结束记录号</param>
        public void SetPaging(int pageIndex, int pageSize, int recordTotal, int recordStart, int recordEnd)
        {
            PageSize = pageSize;
            RecordTotal = recordTotal;
            PageCount = (int)Math.Ceiling((double)recordTotal / pageSize);
            RecordStart = recordStart;
            RecordEnd = recordEnd;
            PageIndex = pageIndex > PageCount ? PageCount : pageIndex;
        }

        #endregion

        #region 必选，用户传参

        /// <summary>
        /// 当前页索引号
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页多少条
        /// </summary>
        public int PageSize { get; set; }

        #endregion

        #region 只读，内部计算

        /// <summary>
        /// 记录总条数
        /// </summary>
        public int RecordTotal { get; private set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// 当前页起始记录号
        /// </summary>
        public int RecordStart { get; private set; }

        /// <summary>
        /// 当前页结束记录号
        /// </summary>
        public int RecordEnd { get; private set; }

        /// <summary>
        /// 是否存在上一页
        /// </summary>
        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }
        /// <summary>
        /// 是否存在下一页
        /// </summary>
        public bool HasNextPage
        {
            get { return (PageIndex < PageCount); }
        }

        #endregion
    }

}
