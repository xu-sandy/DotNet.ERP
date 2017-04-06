using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.ApiParams
{
    public class UserInfoParams : BaseApiParams
    {
        public StoreOperateAuth StoreOperateAuth { get; set; }
    }
    /// <summary>
    ///零售店角色（1:店长、2:营业员、3:收银员、4:数据维护）
    /// </summary>
    public enum StoreOperateAuth
    {
        /// <summary>
        /// 店长
        /// </summary>
        ShopManager = 1,
        /// <summary>
        /// 营业员/导购员
        /// </summary>
        ShoppingGuide = 2,
        /// <summary>
        /// 收银员
        /// </summary>
        Cashier = 3,
        /// <summary>
        /// 数据维护员
        /// </summary>
        DataManager = 4

    }
}
