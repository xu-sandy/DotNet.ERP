using System.Windows;
using System.Windows.Input;
using Pharos.Wpf.Extensions;
using Pharos.POS.Retailing.Models.ViewModels;

namespace Pharos.POS.Retailing
{
    /// <summary>
    /// LockScreen.xaml 的交互逻辑
    /// </summary>
    public partial class LockScreen : Window
    {
        public LockScreen()
        {
            InitializeComponent();
            this.ApplyBindings(this, new LockScreenViewModel());
            this.Loaded += _this_Loaded;
        }

        void _this_Loaded(object sender, RoutedEventArgs e)
        {
            this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
