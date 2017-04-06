using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Logic.WeighDevice;
using Pharos.Utility;
using Pharos.Sys.Entity;
using Pharos.Logic.Entity;

namespace Pharos.WeigtScale.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<VwProduct> odata = new List<VwProduct>();
            for (int i = 0; i < 10; i++)
            {
                VwProduct _odata = new VwProduct()
                {
                    Barcode = "0100000001" + i,
                    ProductCode = "000016" + i,
                    SubUnitId = 117,
                    Title = "商品" + i,
                    SysPrice = 250 + i,
                };
                odata.Add(_odata);
            }
            List<SysDataDictionary> sdatas = new List<SysDataDictionary>();
            SysDataDictionary sdata = new SysDataDictionary()
            {
                DicSN = 117,
                Title = "KG",
            };
            sdatas.Add(sdata);
            JHScaleService service = new JHScaleService();
            var datas = service.DataFormat(odata, sdatas);
            List<string> ips = new List<string>() { "192.168.10.234" };
            var _result = service.TransferData(datas, ips, true);
            string re = _result.Successed + " " + _result.Message;
            Console.WriteLine(re);
        }
    }
}
