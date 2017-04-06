using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.DataSynchronism.ObjectModels.UploadProviders
{
    public class DataSyncSaleOrdersUploadProvider : IUploadProvider<DataSyncSaleOrders>
    {
        public DataSyncResult SaveChanges(IEnumerable<DataSyncSaleOrders> datas)
        {
            try
            {
                using (DataSyncDBContext db = new DataSyncDBContext())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var paySns = db.SaleOrders.Select(o => o.PaySN).Distinct().ToList();
                            var failDatas = new List<DataSyncSaleOrders>();
                            var tempDatas = new List<DataSyncSaleOrders>();
                            foreach (var item in datas)
                            {
                                if (paySns.Exists(p => p == item.PaySN))
                                {
                                    failDatas.Add(item);
                                }
                                else if (tempDatas.Exists(o => o.PaySN == item.PaySN))
                                {
                                    failDatas.Add(item);

                                }
                                else
                                {
                                    tempDatas.Add(item);
                                }
                            }
                            db.SaleOrders.AddRange(tempDatas);
                            db.SaveChanges();
                            transaction.Commit();
                            return new DataSyncResult()
                            {
                                IsSuccess = true,
                                Datas = tempDatas.Select(o => o.PaySN),
                                FailDatas = failDatas
                            };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new DataSyncResult()
                {
                    IsSuccess = false,
                    Message = string.Format("销售主表数据上传失败，服务器异常【{0}】", ex.Message),
                    FailDatas = datas
                };
            }
        }
    }
}
