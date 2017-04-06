using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Pharos.Logic.BLL.ApiPos
{
    public partial class Product
    {
        public Product(Barcode barcode)
        {
            NewPrice = -1;
            Barcode = barcode;
            switch (barcode.Type)
            {
                case BarcodeType.Standard:
                    Num = 1;
                    break;
                case BarcodeType.Custom:
                    Num = 1;
                    break;
                case BarcodeType.WeighCode:
                    Num = Barcode.Weight;
                    NewPrice = Barcode.SalePrices;
                    break;
            }
        }
        [JsonIgnore]
        public Barcode Barcode { get; set; }

        [JsonProperty("Barcode")]
        public string _BarcodeStr { get { return Barcode.Current; } }
        /// <summary>
        /// 销售数量
        /// </summary>
        public decimal Num { get; set; }
        /// <summary>
        /// 前端修改的价格或者称重商品价格，默认为-1
        /// </summary>
        public decimal NewPrice { get; set; }
    }
}
