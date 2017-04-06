using Pharos.Wpf.ViewModelHelpers;
using System.Windows.Input;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class InventoryItem : BaseViewModel
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 产品分类
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 系统售价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>
        public decimal Inventory { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        public string ProductCode { get; set; }

        public ICommand InShoppingCartCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    PosViewModel.Current.Barcode = Barcode;
                });
            }
        }
    }
}
