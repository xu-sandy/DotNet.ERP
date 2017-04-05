using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pharos.POS.Retailing.Models.ApiParams;
using System.Collections.ObjectModel;
using Pharos.POS.Retailing.Models.ViewModels.Pay;


namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// ZhiFuWanCheng.xaml 的交互逻辑
    /// </summary>
    public partial class ZhiFuWanCheng : DialogWindow02
    {
        public ZhiFuWanCheng(decimal _amount, decimal receipt, decimal change, IEnumerable<MultiPayItemViewModel> payItems, PayAction payAction, DateTime orderTime)
        {
            InitializeComponent();
            this.InitDefualtSettings();
            var model = new PayComplete(_amount, receipt, change, payItems, payAction, orderTime);
            this.ApplyBindings(this, model);
           // this.Closing += _This_Closing;
        }

        //private void _This_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    PosViewModel.Current.ClearOrder.Execute(null);
        //}
    }
}
