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
    /// ProductEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ProductEditor : DialogWindow02
    {
        public ProductEditor(ProductEdit model, int mode)
        {
            InitializeComponent();
            //  this.InitDefualtSettings();
            this.ApplyBindings(this, model);
            this.ApplyHotKeyBindings();
            CurrentModel = model;
            if (mode == 0)
            {
                FocusControl = txtPrice;
            }
            else
            {
                FocusControl = txtnum;

            }
            this.PreviewKeyDown += _this_PreviewKeyDown;
            this.Loaded += _this_Loaded;
        }
        void _this_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(FocusControl);
        }

        void _this_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var win = sender as Window;
            switch (e.Key)
            {
                case Key.Escape:
                    win.Close();
                    break;
                case Key.Y:
                    txtPrice.Focus();
                    txtnum.Focus();
                    CurrentModel.Confirm.Execute(null);
                    e.Handled = true;
                    break;
                case Key.Add:
                case Key.Up:
                case Key.PageUp:
                case Key.VolumeUp:
                    CurrentModel.NumAdd.Execute(null);
                    e.Handled = true;
                    break;
                case Key.Down:
                case Key.PageDown:
                case Key.Subtract:
                case Key.VolumeDown:
                    CurrentModel.NumDec.Execute(null);
                    e.Handled = true;
                    break;
            }
        }
        ProductEdit CurrentModel { get; set; }

        internal IInputElement FocusControl { get; private set; }


    }
}
