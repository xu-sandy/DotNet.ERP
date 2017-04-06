using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Entity.View;

namespace Pharos.Logic.OMS.IDAL
{
    /// <summary>
    /// 商家支付主密钥接口
    /// </summary>
    public interface ITradersPaySecretKeyRepository : IBaseRepository<TradersPaySecretKey>
    {
        /// <summary>
        /// 获取CID
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        List<PayLicense> getListCID(string keyword, string strW = "");

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <param name="ChannelNo"></param>
        /// <returns></returns>
        List<ViewPayChannelDetail> GetPayManner(int ChannelNo);

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="CurrentPage">当前页,从1开始,不是0</param>
        /// <param name="PageSize">每页显示多少条数据</param>
        /// <param name="strw">where条件</param>
        /// <param name="Count">总数</param>
        /// <returns></returns>
        List<ViewTradersPaySecretKey> getPageList(int CurrentPage, int PageSize, string strw, out int Count,string OrderBy);
    }
}
