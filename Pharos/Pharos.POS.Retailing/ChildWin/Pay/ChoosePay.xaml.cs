﻿using Pharos.POS.Retailing.Models;
using Pharos.POS.Retailing.Models.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pharos.POS.Retailing.ChildWin.Pay
{
    /// <summary>
    /// 选择支付方式控件
    /// </summary>
    public partial class ChoosePay : ItemsControl
    {
        public ChoosePay()
        {
            InitializeComponent();
            this.ItemsSource = Global.Payways;
            Global.PaywaysRefreshEvent += Global_PaywaysRefreshEvent;

        }
        public Action<PayItem> CallBack { get; set; }
        private void Global_PaywaysRefreshEvent(object obj)
        {
            this.ItemsSource = Global.Payways;
        }

        private void SelectPay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var lbl = sender as Label;
            if (CallBack != null)
                CallBack(lbl.DataContext as PayItem);
        }
    }
}