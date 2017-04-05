using System.Collections.Generic;

namespace Pharos.POS.Retailing.Models.ApiReturnResults
{
    public class ApiProCategoryReturn
    {
        /// <summary>
        /// 分类编号
        /// </summary>
        public int CategorySN { get; set; }
        /// <summary>
        /// 上级分类编号
        /// </summary>
        public int CategoryPSN { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 分类集合
        /// </summary>
        public IEnumerable<ApiProCategoryReturn> Childrens { get; set; }

    }
}
