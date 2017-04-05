﻿using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using System;
using System.Threading.Tasks;

namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// Rijie.xaml 的交互逻辑
    /// </summary>
    public partial class RiJie : DialogWindow02
    {
        public RiJie()
        {
            InitializeComponent();
            this.InitDefualtSettings();
            var model = new DayReportViewModel();
            this.ApplyBindings(this, model);
        }

        private void DatePicker_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Enter:
                    m.Focus();
                    break;
            }
        }
    }
}
