using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using Pharos.Wpf.HotKeyHelper;
using System.Windows;
using System.Windows.Input;

namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// SetWeigh.xaml 的交互逻辑
    /// </summary>
    public partial class SetWeigh : ToastWindow
    {
        public SetWeigh(SetWeightViewModel model)
        {
            InitializeComponent();

            this.ApplyBindings(this, model);
            this.InitDefualtSettings();
            //this.ApplyHotKeyBindings();
            //this.PreviewKeyDown += _this_PreviewKeyDown;

            //this.Loaded += SetWeigh_Loaded;
            this.Activated += SetWeigh_Activated;
        }

        void SetWeigh_Activated(object sender, System.EventArgs e)
        {
            txtWeight.Focus();
        }

        private void _this_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var win = sender as Window;
            switch (e.Key)
            {
                case Key.Escape:
                    win.Close();
                    break;
            }
        }

        void SetWeigh_Loaded(object sender, RoutedEventArgs e)
        {
            txtWeight.Focus();
        }
    }
}
