﻿using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using System;
using System.Threading.Tasks;

namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// ZhengDanZheKou.xaml 的交互逻辑
    /// </summary>
    public partial class ZhengDanZheKou : DialogWindow02
    {
        public ZhengDanZheKou(decimal amount)
        {
            InitializeComponent();
            this.InitDefualtSettings();
            var model = new AllOrderDiscount(amount);
            this.ApplyBindings(this, model);
        }


    }
}
