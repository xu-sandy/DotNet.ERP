﻿using Pharos.POS.Retailing.Models;
using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.PosModels;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Extensions;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Pharos.POS.Retailing.Models.HotKeyCommands;

namespace Pharos.POS.Retailing
{
    /// <summary>
    /// PosWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PosWindow : Window, IPosDataGrid, IBarcodeControl
    {
        public PosWindow(string appInfo, string winIcon, OperatingStatus status)
        {
            InitializeComponent();
            var monitor = Pharos.Wpf.Monitor.AllMonitors.Where(o => o.IsPrimary).FirstOrDefault();
            if (monitor != null)
            {
                this.Top = monitor.Bounds.Top;
                this.Left = monitor.Bounds.Left;
                this.Width = monitor.Bounds.Width;
                this.Height = monitor.Bounds.Height;
                this.ResizeMode = System.Windows.ResizeMode.NoResize;
            }

            Application.Current.MainWindow = this;
            this.ApplyHotKeyBindings();

            CurrentModel = new PosViewModel(appInfo, winIcon, status);
            this.ApplyBindings(this, CurrentModel);
            CurrentIInputElement = txtBarcode;
            CurrentGrid = dgOrderList;
            this.Loaded += PosPage_Loaded;
            this.PreviewKeyDown += PosWindow_PreviewKeyDown;
            this.Closing += PosWindow_Closing;
            this.StateChanged += PosWindow_StateChanged;
        }

        void PosWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
        }

        void PosWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //0714 有未结算订单不允许退出
            //if (CurrentModel != null && CurrentModel.OrderList.Count > 0)
            //{
            //    Toast.ShowMessage("请先结算，再退出！", Application.Current.MainWindow);
            //    e.Cancel = true;
            //    return;
            //}
            //160712 系统存在挂单数据是不允许退出程序
            if (CurrentModel != null && CurrentModel.HandBillNum > 0)
            {
                try
                {
                    Toast.ShowMessage("存在挂单数据，不允许退出程序！", Application.Current.MainWindow);
                    e.Cancel = true;
                }
                catch { }
            }
            else
            {
                try
                {
                    Confirm.ShowMessage("确定退出程序？", null, (o =>
                    {
                        if (o == ConfirmMode.Confirmed)
                        {
                            CurrentModel.ClearOrder.Execute(null);
                            this.Hide();
                            Task.Factory.StartNew(() =>
                            {
                                Thread.Sleep(1000);
                                Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    Application.Current.Shutdown();
                                }));
                            });
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }));
                }
                catch { }
            }
        }

        void PosWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                //case Key.Down:
                //    if (CurrentGrid.SelectedIndex == -1 && CurrentModel.OrderList.Count > 0)
                //    {
                //        CurrentModel.SetScrollIntoView(0);
                //    }
                //    else if (CurrentModel.OrderList.Count > 0 && CurrentGrid.SelectedIndex < CurrentModel.OrderList.Count)
                //    {
                //        CurrentModel.SetScrollIntoView(CurrentGrid.SelectedIndex + 1);
                //    }
                //    e.Handled = true;
                //    break;
                //case Key.Up:
                //    if (CurrentGrid.SelectedIndex == -1 && CurrentModel.OrderList.Count > 0)
                //    {
                //        CurrentModel.SetScrollIntoView(CurrentModel.OrderList.Count - 1);
                //    }
                //    else if (CurrentModel.OrderList.Count > 0 && CurrentGrid.SelectedIndex > 0)
                //    {
                //        CurrentModel.SetScrollIntoView(CurrentGrid.SelectedIndex - 1);
                //    }
                //    e.Handled = true;
                //    break;
                case Key.Delete:
                    if (CurrentGrid.SelectedItem != null)
                    {
                        var product = (Product)CurrentGrid.SelectedItem;
                        product.RemoveCommand.Execute(null);
                        e.Handled = true;
                    }
                    break;
            }
        }



        PosViewModel CurrentModel { get; set; }
        public DataGrid CurrentGrid { get; set; }
        public IInputElement CurrentIInputElement { get; set; }

        void PosPage_Loaded(object sender, RoutedEventArgs e)
        {
            // this.ThreadFilterMessage();
            Keyboard.Focus(txtBarcode);
            Task.Factory.StartNew(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    CurrentModel.ReadHandBillNumber.Execute(null);
                }));
            });
        }

        private void SaleStatusTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (CurrentModel.PosStatus)
            {
                case PosStatus.Normal:
                    CurrentModel.PosStatus = PosStatus.Gift;
                    break;
                case PosStatus.Gift:
                    CurrentModel.PosStatus = PosStatus.Normal;
                    break;
            }
        }

        private void PrintStatusTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (CurrentModel.PrintStatus)
            {
                case PrintStatus.Open:
                    CurrentModel.PrintStatus = PrintStatus.Close;
                    break;
                case PrintStatus.Close:
                    CurrentModel.PrintStatus = PrintStatus.Open;
                    break;
            }
        }

        private void txtBarcode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var ctrl = sender as TextBox;
            if (e.Key == Key.Enter && string.IsNullOrEmpty(ctrl.Text.Trim()) && !ctrl.IsReadOnly)
            {
                if (!PosViewModel.Current.IsClearStatus && PosViewModel.Current.Num > 0)
                    new XianJinZhiFuCommand().Handler(this);
            }
            if (e.Key == Key.Enter)
            {
                ctrl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                Keyboard.Focus(ctrl);
            }
        }

        private void KeyBoardSwitch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (PosViewModel.Current.InputMode)
            {
                case HotkeyMode.Close:
                    PosViewModel.Current.InputMode = HotkeyMode.Open;
                    break;
                case HotkeyMode.Open:
                    PosViewModel.Current.InputMode = HotkeyMode.Close;
                    break;
            }
        }

        private void MinAction_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }



    }
}