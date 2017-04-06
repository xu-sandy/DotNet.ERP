using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.ApiPos
{
    public class Barcode
    {
        public Barcode(string barcode)
        {
            Current = barcode.Trim().Replace(" ", "");
            Analysis();
        }
        public string Current { get; set; }

        public BarcodeType Type { get; set; }

        public decimal SalePrices { get; set; }

        public decimal Total { get; set; }

        public decimal Weight { get; set; }

        public string ProductCode { get; set; }

        public decimal Price { get; set; }

        public string StoreId { get; set; }

        private void Analysis()
        {
            switch (Current.Length)
            {
                case 10:
                    Type = BarcodeType.Custom;
                    break;
                case 13:
                    Type = BarcodeType.Standard;
                    break;
                case 18:
                    Type = BarcodeType.WeighCode;
                    StoreId = Current.Substring(0, 2);
                    ProductCode = Current.Substring(2, 6);
                    Weight = Convert.ToInt32(Current.Substring(8, 5)) / 1000.0m;
                    Total = Convert.ToInt32(Current.Substring(13, 5)) / 100.0m;
                    Price = Total / Weight;
                    break;
                default:
                    throw new BarcodeException(string.Format("{0},该条码未能正常解析！", Current));
            }
        }

        public static bool operator ==(Barcode a, Barcode b)
        {
            return a.Current == b.Current;
        }
        public static bool operator !=(Barcode a, Barcode b)
        {
            return a.Current != b.Current;
        }
        public override bool Equals(object obj)
        {
            var barcode = (obj as Barcode);
            return this.Equals(barcode);
        }
        public bool Equals(Barcode barcode)
        {
            if (barcode == null)
            {
                return false;
            }
            return this.Current.Equals(barcode.Current);

        }
        public bool Equals(string barcode)
        {
            return this.Current.Equals(barcode);
        }

        public override int GetHashCode()
        {
            return this.Current.GetHashCode();
        }
        public override string ToString()
        {
            return Current;
        }

    }

    public enum BarcodeType
    {
        /// <summary>
        /// 标准13位国际条码
        /// </summary>
        Standard = 0,
        /// <summary>
        /// 称重条码18位条码，2位商店编号+6位货号+5位重量（单位：g）+5位总价（单位:分）
        /// </summary>
        WeighCode = 1,
        /// <summary>
        /// 非国际商品条码，客户自定义10位，种类6位（大中小）+4位流水号
        /// </summary>
        Custom = 2,
    }

    public class BarcodeException : Exception
    {
        public BarcodeException()
            : base("条码解析异常")
        {
        }
        public BarcodeException(string msg)
            : base(msg)
        {
        }
    }
}
