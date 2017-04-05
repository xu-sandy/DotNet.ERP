﻿using Pharos.POS.Retailing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.PosModels;
using System.Threading.Tasks;
using System.Threading;

namespace Pharos.POS.Retailing.ChildWin.Pay
{
    /// <summary>
    /// 选择支付方式窗口
    /// </summary>
    public partial class ChoosePayWindow : Window
    {
        public ChoosePayWindow(PayAction action)
        {
            InitializeComponent();
            this.itemsPay.CallBack = (o) =>
            {
                PayItem = o;
                txtChoosePay.Text = "已选择“" + o.Title + "”";
                txtAmount.SelectAll();
            };
            this.DataContext = this;
            this.Loaded += ChoosePayWindow_Loaded;
            this.InitDefualtSettings();
            PayAction = action;
            this.MouseUp += ChoosePayWindow_MouseUp;
        }

        void ChoosePayWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            txtAmount.Focus();
        }

        void ChoosePayWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtAmount.Focus();
            this.Left = this.Owner.Left + 20;
            this.Top = this.Owner.Top + 220;
        }
        public PayAction PayAction { get; set; }
        public decimal Amount { get; set; }
        public PayItem PayItem { get; set; }

        public bool isCannel = true;

        internal void PosCancelButton_Click(object sender, RoutedEventArgs e)
        {
            PayItem = null;
            isCannel = true;
            this.Close();
        }

        private void PosOkButton_Click(object sender, RoutedEventArgs e)
        {
            isCannel = false;
            if (PayAction != Models.PosModels.PayAction.Sale && (PayItem.Mode == PayMode.RongHeCustomerDynamicQRCodePay || PayItem.Mode == PayMode.RongHeDynamicQRCodePay))
            {
                Toast.ShowMessage("该支付暂时不支持在退换货中使用！", this);
                return;
            }
            if (PayItem == null)
            {
                Toast.ShowMessage("请选择支付方式！", this);
                txtAmount.Select(txtAmount.Text.Length, 0);
                return;
            }
            this.Close();
        }

        public void SetPayItem(PayMode mode)
        {
            IEnumerable<PayItem> pays = this.itemsPay.ItemsSource as IEnumerable<PayItem>;
            var payItem = pays.FirstOrDefault(o => o.Mode == mode);
            if (payItem != null)
            {
                switch (PayAction)
                {
                    case Models.PosModels.PayAction.RefundAll:
                    case Models.PosModels.PayAction.Refund:
                    case Models.PosModels.PayAction.Change:
                        if (payItem.Mode != PayMode.CashPay)
                        {
                            Toast.ShowMessage("退换货当前只允许现金支付！", this);
                            return;
                        }
                        break;
                }
                PayItem = payItem;
                txtChoosePay.Text = "已选择" + PayItem.Title;
            }
            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(300);
            //    this.Dispatcher.Invoke(new Action(() =>
            //    {
            //        txtAmount.Focus();
            //    }));
            //});
        }
    }
}
