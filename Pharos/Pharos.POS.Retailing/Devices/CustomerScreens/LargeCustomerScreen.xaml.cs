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
using Pharos.Wpf.Extensions;
using System.Windows.Shapes;
using Pharos.Wpf;
using Pharos.POS.Retailing.Models.PosModels;

namespace Pharos.POS.Retailing.Devices.CustomerScreens
{
    /// <summary>
    /// LargeCustomerScreen.xaml 的交互逻辑
    /// </summary>
    public partial class LargeCustomerScreen : Window, ICustomerScreen
    {
        public LargeCustomerScreen()
        {
            InitializeComponent();
            this.Focusable = false;
            ShowInTaskbar = false;
            var monitor = Monitor.AllMonitors.Where(o => !o.IsPrimary).FirstOrDefault();
            if (monitor != null)
            {
                this.Top = monitor.WorkingArea.Top;
                this.Left = monitor.WorkingArea.Left;
                this.Width = monitor.WorkingArea.Width;
                this.Height = monitor.WorkingArea.Height;
                this.ResizeMode = ResizeMode.NoResize;
            }
            var images = new List<ImageSource>();
            images.Add(new BitmapImage(new Uri(@"..\..\Images\CustomerScreen\erp.png", UriKind.Relative)));
            lock (adv.Images)
            {
                adv.Images = images;
            }
            adv.Timer(5000);
        }

        public void Show(Models.ViewModels.PosViewModel datas)
        {
            this.DataContext = datas;
            datas.PropertyChanged += Datas_PropertyChanged;
            datas.ViewChanged = (index) =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    var svr = VisualTreeHelper.GetChild(cproducts, 0) as ScrollViewer;
                    svr.ScrollToBottom();
                }));
            };
            this.Show();
        }

        private void Datas_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MultiPayItemViewModel")
            {
                if (Models.ViewModels.PosViewModel.Current.MultiPayItemViewModel == null)
                {
                    payPanal.Visibility = Visibility.Collapsed;
                    adv.Visibility = Visibility.Visible;
                    gp.Visibility = Visibility.Visible;
                }
                else
                {
                    payPanal.Visibility = Visibility.Visible;
                    adv.Visibility = Visibility.Collapsed;
                    gp.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
