using Pharos.POS.Retailing.Models.ViewModels.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Pharos.POS.Retailing.XamlConverters
{
    public class PayItemViewDataTemplateSelector : DataTemplateSelector
    {
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            if (item != null && item is MultiPayItemViewModel)
            {
                MultiPayItemViewModel multiPayViewModel = item as MultiPayItemViewModel;

                switch (multiPayViewModel.PayItem.Mode)
                {
                    case Models.PosModels.PayMode.StoredValueCard:
                        return StoredValueCardTpl;
                    case Models.PosModels.PayMode.UnionPayCTPOSM:
                        return UnionPayTpl;
                    case Models.PosModels.PayMode.RongHeDynamicQRCodePay:
                        return RongHeDynamicQRCodePayTpl;
                    case Models.PosModels.PayMode.RongHeCustomerDynamicQRCodePay:
                        return RongHeCustomerDynamicQRCodePayTpl;
                    default:
                        return PayDefaultTpl;
                }
            }
            return null;
        }

        public DataTemplate PayDefaultTpl { get; set; }
        public DataTemplate UnionPayTpl { get; set; }
        public DataTemplate StoredValueCardTpl { get; set; }
        public DataTemplate RongHeDynamicQRCodePayTpl { get; set; }
        public DataTemplate RongHeCustomerDynamicQRCodePayTpl { get; set; }
    }
}
