using Pharos.POS.Retailing.Models;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Linq;

namespace Pharos.POS.Retailing.Extensions
{
    public static class WindowExtensions
    {
        public static void InitDefualtSettings(this Window _this)
        {
            _this.PreviewKeyDown += _this_PreviewKeyDown;
            _this.Loaded += _this_Loaded;
            _this.Activated += _this_Activated;

            _this.ApplyHotKeyBindings();
        }

        private static void _this_Activated(object sender, EventArgs e)
        {
            (sender as Window).MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        static void _this_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(150);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    (sender as Window).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    if (Keyboard.FocusedElement is TextBox)
                    {
                        (Keyboard.FocusedElement as TextBox).SelectAll();
                    }
                }));
            });
        }


        public static void _this_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var win = sender as Window;
            switch (e.Key)
            {
                case Key.Escape:
                    win.Close();
                    break;
            }
        }
    }
}
