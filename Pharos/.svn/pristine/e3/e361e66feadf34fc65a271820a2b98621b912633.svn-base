using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.ApiPos
{
    public abstract class BaseShoppingCart
    {
        public List<Product> Products { get; set; }
        /// <summary>
        /// 根据条码添加商品，称重商品不合并
        /// </summary>
        /// <param name="barcode">条码</param>
        public virtual Product PutProduct(Barcode barcode)
        {
            var product = new Product(barcode);
            if (!Products.Exists(o => o.Barcode == barcode) || barcode.Type == BarcodeType.WeighCode)
            {
                Products.Add(product);
            }
            else
            {
                product = Products.FirstOrDefault(o => o.Barcode == barcode);
                product.Num++;
            }
            return product;
        }

        public virtual Product EditProduct(Barcode barcode, decimal num, decimal salePrice)
        {
            if (num == 0)
                RemoveProduct(barcode);
            var info = Products.FirstOrDefault(o => o.Barcode == barcode);
            EditProductNum(info, num);
            EditProductSale(info, salePrice);
            return info;
        }
        public virtual Product EditProductNum(Barcode barcode, decimal num)
        {
            var info = Products.FirstOrDefault(o => o.Barcode == barcode);
            return EditProductNum(info, num);
        }
        public virtual Product EditProductNum(Product product, decimal num)
        {

            if (product != null)
            {
                if (num == 0)
                    RemoveProduct(product.Barcode);
                product.Num = num;
            }
            return product;
        }
        public virtual Product EditProductSale(Barcode barcode, decimal salePrice)
        {
            var info = Products.FirstOrDefault(o => o.Barcode == barcode);
            return EditProductSale(info, salePrice);
        }
        public virtual Product EditProductSale(Product product, decimal salePrice)
        {
            if (product != null)
            {
                product.NewPrice = salePrice;
            }
            return product;
        }

        public virtual void RemoveProduct(Barcode barcode)
        {
            var result = Products.RemoveAll(o => o.Barcode == barcode);
        }

        public virtual Object MatchingPromotionRule()
        {
            return null;
        }


    }
}
