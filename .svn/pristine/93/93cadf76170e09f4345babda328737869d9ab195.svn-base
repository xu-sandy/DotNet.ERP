using Pharos.Logic.ApiData.Pos.ValueObject;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Pharos.Logic.ApiData.Pos.Extensions
{
    /// <summary>
    /// 集合扩展
    /// </summary>
    public static class CollectionsExtension
    {
        /// <summary>
        /// 扩展DbRawSqlQuery<T>支持分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="datas">数据源</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">分页索引</param>
        /// <returns>分页结果</returns>
        public static PageResult<T> ToPageList<T>(this DbRawSqlQuery<T> datas, int pageSize = 50, int pageIndex = 1)
        {
            var collections = datas.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var total = datas.Count();
            var pageCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
            var pager = new PageInfo()
            {
                Index = pageIndex,
                Size = pageSize,
                Total = total,
                PageCount = pageCount
            };
            var result = new PageResult<T>()
            {
                Datas = collections,
                Pager = pager
            };
            return result;
        }
    }
}
