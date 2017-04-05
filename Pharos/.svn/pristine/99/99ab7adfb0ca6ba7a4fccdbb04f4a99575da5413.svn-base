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
using System.Threading.Tasks;
using Pharos.Wpf.Extensions;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;



namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// DingDanXiangQing.xaml 的交互逻辑
    /// </summary>
    public partial class DingDanXiangQing : DialogWindow02
    {
        public DingDanXiangQing(string paySn)
        {
            InitializeComponent();
            this.InitDefualtSettings();
            var model = new OrderDetailsViewModel(paySn);
            this.ApplyBindings(this, model);
        }
    }
}
