using System.Windows;
using System.Windows.Controls;
using Pharos.Frame.Wpf.Extensions;
using Pharos.Barcode.Retailing.Models;

namespace Pharos.Barcode.Retailing.ChildTabContentControls
{
    /// <summary>
    /// SystemManagement.xaml 的交互逻辑
    /// </summary>
    public partial class SystemManagement : UserControl
    {
        public SystemManagement()
        {
            InitializeComponent();
            this.Loaded += SystemManagement_Loaded;
        }

        void SystemManagement_Loaded(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            win.ApplyBindings(printerSettings, BarCodePrinter.Current);
            win.ApplyBindings(serverSettings, SystemConfiguration.Current);
        }
    }
}
