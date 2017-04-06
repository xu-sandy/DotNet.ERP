using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pharos.POS.Retailing
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : LoginWindow
    {
        LoginViewModel currentModel;
        public Login()
        {
            InitializeComponent();
            var monitor = Monitor.AllMonitors.Where(o => o.IsPrimary).FirstOrDefault();
            if (monitor != null)
            {
                this.Top = monitor.WorkingArea.Top + ((monitor.WorkingArea.Height - 400) / 2);
                this.Left = monitor.WorkingArea.Left + ((monitor.WorkingArea.Width - 700) / 2);
                this.ResizeMode = System.Windows.ResizeMode.NoResize;
            }
            this.InitDefualtSettings();
            Application.Current.MainWindow = this;
            currentModel = new LoginViewModel();
            this.ApplyBindings(this, currentModel);
            this.Loaded += Login_Loaded;
        }

        void Login_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    //to connect Server get data
                    var result = ApiManager.Post<object, ApiRetrunResult<AppInfo>>(@"api/AppInfo", "");
                    if (result.Code == "200")
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            // 赋值
                            currentModel.AppName = result.Result.Name;
                            currentModel.Title = result.Result.FullName;
                        }));
                    }
                    else
                    {
                        Toast.ShowMessage(result.Message, Application.Current.MainWindow);
                    }
                }
                catch { }
            });
            Keyboard.Focus(txtAccount);
            Global.LoadDefualtItems();

        }

    }
}
