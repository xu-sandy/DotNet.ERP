using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Pharos.Wpf.HotKeyHelper;
using System.Threading;
using System.Windows;
using Pharos.POS.Retailing.Models.ApiReturnResults;


namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// ChaDingDan.xaml 的交互逻辑
    /// </summary>
    public partial class ChaDingDan : DialogWindow02
    {
        public ChaDingDan()
        {
            InitializeComponent();
            // this.InitDefualtSettings();
            var model = new FindBills();
            this.ApplyBindings(this, model);
            this.PreviewKeyDown += Pharos.POS.Retailing.Extensions.WindowExtensions._this_PreviewKeyDown;
            this.ApplyHotKeyBindings();
            this.Loaded += ChaDingDan_Loaded;
        }

        void ChaDingDan_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(150);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Keyboard.Focus(txtpaysn);
                    txtpaysn.SelectAll();
                }));
            });
        }

        private void DatePicker_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Enter:
                    Keyboard.Focus(txtpaysn);
                    break;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var model = DataView.SelectedItem as OrderInfoModel;
            if (model != null)
            {
                model.CheckDetail.Execute(null);
            }
        }
    }
}
