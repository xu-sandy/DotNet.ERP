using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Frame.Wpf.ViewModels;
using Pharos.Utility;

namespace Pharos.Barcode.Retailing.Dtos
{
    [Excel("条码数据")]
    public class ProductDto : BaseViewModel
    {
        private bool? isChecked = false;
        public bool? IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                this.OnPropertyChanged(o => o.IsChecked);
            }
        }
        [Excel("条码", 1)]
        public string Barcode { get; set; }
        [Excel("货号", 2)]
        public string ProductCode { get; set; }
        public string Category { get; set; }
        [Excel("商品名称", 3)]
        public string ExportTitle { get; set; }

        public string Title { get; set; }
        public string Brand { get; set; }
        [Excel("单位", 4)]
        public string Unit { get; set; }
        [Excel("价格", 5)]
        public decimal SysPrice { get; set; }

        public bool IsWeigh { get; set; }
    }
}
