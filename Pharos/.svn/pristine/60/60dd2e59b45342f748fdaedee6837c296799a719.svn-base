using Pharos.Infrastructure.Data.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.Logic.OMS.BLL
{
    /// <summary>
    /// 订单助手
    /// </summary>
    public class OrderHelper
    {
        /// <summary>
        /// 防止创建类的实例
        /// </summary>
        private OrderHelper() { }

        private static readonly object Locker = new object();
        private static int _sn = -1;
        private static int _ordersn = -1;
        private static int _num = 0;
        private const string _payKey = "pay_";

        /// <summary>
        /// 生成支付订单流水号
        /// </summary>
        /// <returns></returns>
        public static string GetMaxTradeNo()
        {
            lock (Locker)  //lock 关键字可确保当一个线程位于代码的临界区时，另一个线程不会进入该临界区。
            {
                if (_sn == -1)
                {
                    var svc = new TradeOrderService();
                    var maxNo = svc.GetMaxTradeNo();
                    _sn = maxNo;
                }
                if (_sn == int.MaxValue)
                    _sn = 0;
                else
                    _sn++;
                //Thread.Sleep(100);

                return DateTime.Now.ToString("yyyyMMdd") + _sn.ToString().PadLeft(10, '0');
            }
        }
        /// <summary>
        /// 生成商户订单号 fishtodo:把订单号修改为短一点，启动时先去数据库里取最大值
        /// </summary>
        /// <returns></returns>
        public static string GetMaxOutOrderNo()
        {
            lock (Locker)  //lock 关键字可确保当一个线程位于代码的临界区时，另一个线程不会进入该临界区。
            {
                if (_ordersn == -1)
                {
                    _ordersn = 0;
                }
                if (_num == 0)
                {
                    var payNum = new PayNumber();
                    _num = payNum.GetMaxNum(_payKey);
                    payNum.SetNext(_payKey, _num);
                }
                if (_ordersn == int.MaxValue)
                    _ordersn = 0;
                else
                    _ordersn++;
                //Thread.Sleep(100);
                return string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMdd") ,_num, _ordersn.ToString().PadLeft(10, '0'));
            }
        }

        
    }
    /// <summary>
    /// 生成当天支付流水号所属的编号值（防止程序停止时，订单序号重复）
    /// </summary>
    public class PayNumber{
        private string _cacheDirPath = string.Empty;
        private const string _dateFormat = "yyyyMMdd";
        private const string _tempFormat = "{0}_{1}";

        /// <summary>
        /// 支付缓存文件存储路径
        /// </summary>
        public string CacheFilePath {
            get
            {
                if (string.IsNullOrEmpty(_cacheDirPath))
                {
                    _cacheDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "QctCache");
                }
                return _cacheDirPath;
            }
        }
        /// <summary>
        /// 获取订单的最大一级编号（yyyyMMdd+一级编号（最多2位）+二级编号（最多10位）），如：2017010111020202020
        /// </summary>
        public int GetMaxNum(string key) {
            var fileName = GetFileName(key, DateTime.Now);

            var num = 1;
            if (!File.Exists(fileName))
            {
                SaveToFile(key, fileName, num);
            }
            else
            {
                num = FileReadWrite.Read(fileName, FileMode.Open, (fs) =>
                {
                    byte[] buffer = new byte[4];
                    fs.Read(buffer, 0, 4);
                    return BitConverter.ToInt32(buffer, 0);
                });
            }
            return num;
        }
        /// <summary>
        /// 当天支付流水号所属的编号值累计加1
        /// </summary>
        /// <param name="key"></param>
        /// <param name="num"></param>
        public void SetNext(string key,int num)
        {
            var fileName = GetFileName(key, DateTime.Now);
            SaveToFile(key, fileName, num+1);
        }
        /// <summary>
        /// 订单编号标识文件+1
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="num"></param>
        public void SaveToFile(string key, string fileName,int num)
        {
            try
            {
                FileReadWrite.Write(fileName, FileMode.Create, (fs) =>
                {
                    var buffer = BitConverter.GetBytes(num);
                    fs.Write(buffer, 0, buffer.Length);
                });
                if (num == 1)
                {
                    //清除昨天的流水文件
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            var delfileName = GetFileName(key, DateTime.Now.AddDays(-1));
                            delfileName = Path.Combine(CacheFilePath, delfileName);
                            if (File.Exists(delfileName))
                            {
                                File.Delete(delfileName);
                            }
                        }
                        catch { }
                    });
                }
            }
            catch(Exception ex){ }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private string GetFileName(string key, DateTime date)
        {
            var fileName = string.Format(_tempFormat, key, date.ToString(_dateFormat));
            return Path.Combine(CacheFilePath, fileName);
        }
    }
}
