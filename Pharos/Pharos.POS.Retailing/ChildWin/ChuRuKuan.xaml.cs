﻿using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using System;
using System.Threading.Tasks;

namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// ChuRuKuan.xaml 的交互逻辑
    /// </summary>
    public partial class ChuRuKuan : DialogWindow02
    {
        public ChuRuKuan()
        {
            InitializeComponent();
            this.InitDefualtSettings();
            var model = new TakeOutOrDepositMoney();
            this.ApplyBindings(this, model);
        }
    }
}
