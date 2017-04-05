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
using Pharos.Frame.Wpf.Extensions;
using Pharos.Barcode.Retailing.Models;

namespace Pharos.Barcode.Retailing.ChildPages
{
    /// <summary>
    /// SetRepeatNum.xaml 的交互逻辑
    /// </summary>
    public partial class SetRepeatNum : Window
    {
        public SetRepeatNum(BarcodePrintModel model)
        {
            InitializeComponent();
            this.Loaded += SetRepeatNum_Loaded;
            CurrentModel = model;
        }

        BarcodePrintModel CurrentModel { get; set; }

        void SetRepeatNum_Loaded(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            win.ApplyBindings(this, CurrentModel);
        }
    }
}
