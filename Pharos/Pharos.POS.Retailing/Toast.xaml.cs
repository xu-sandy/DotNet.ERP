﻿using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pharos.POS.Retailing
{
    /// <summary>
    /// Toast.xaml 的交互逻辑
    /// </summary>
    public partial class Toast : ToastWindow
    {
        public Toast(ToastMessage model)
        {
            InitializeComponent();
            this.PreviewKeyDown += Toast_PreviewKeyDown;
            this.Loaded += Toast_Loaded;
            CurrentModel = model;
            this.ApplyBindings(this, CurrentModel);
        }
        public ToastMessage CurrentModel { get; set; }

        void Toast_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                while (CurrentModel.Seconds >= 0)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        CurrentModel.Seconds--;
                    }));
                    Thread.Sleep(1000);
                }
            });
        }
        public static void ShowMessage(ToastMessage model, Window owner)
        {
            Toast win = new Toast(model);
            if (owner != null && owner.Visibility == Visibility.Visible)
            {
                win.Owner = owner;
            }
            win.ShowDialog();
        }
        public static void ShowMessage(string msg, Window owner)
        {
            ToastMessage model = new ToastMessage()
            {
                Message = msg,
            };
            ShowMessage(model, owner);
        }
        void Toast_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                case Key.Escape:
                case Key.Space:
                case Key.Delete:
                case Key.Back:
                    CurrentModel.Seconds = 0;
                    break;
            }
            e.Handled = true;
        }
    }
}
