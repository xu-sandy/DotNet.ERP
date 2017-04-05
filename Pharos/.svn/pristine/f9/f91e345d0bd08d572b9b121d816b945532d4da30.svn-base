using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
namespace Pharos.Logic.BLL
{
    public class PayNotifyResultService:BaseService<PayNotifyResult>
    {
        /// <summary>
        /// 写入唯一的一条记录
        /// </summary>
        /// <param name="obj"></param>
        public static void AddOne(PayNotifyResult obj)
        {
            try
            {
                if (CurrentRepository.IsExist(o => o.PaySN == obj.PaySN)) return;
                if (Add(obj).Successed)
                    Log.WriteInfo("支付成功");
                else
                    Log.WriteInfo("支付失败");
            }
            catch(Exception ex)
            {
                Log.WriteError("支付异常", ex);
            }
        }
    }
}
