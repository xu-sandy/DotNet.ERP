﻿using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pharos.POS.Retailing
{
    /// <summary>
    /// Confirm.xaml 的交互逻辑
    /// </summary>
    public partial class Confirm : ToastWindow
    {
        public Confirm(ConfirmMessage message)
        {
            InitializeComponent();
            this.PreviewKeyDown += Confirm_PreviewKeyDown;
            CurrrentModel = message;
            this.ApplyBindings(this, CurrrentModel);
        }

        protected override void CloseClick(object sender, RoutedEventArgs e)
        {
            Cancel_Click(null, null);
        }
        ConfirmMessage CurrrentModel { get; set; }
        void Confirm_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Y:
                case Key.Enter:
                    Confirm_Click(null, null);
                    break;
                case Key.N:
                case Key.Escape:
                    Cancel_Click(null, null);
                    break;
            }
            e.Handled = true;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (CurrrentModel.CallBack != null)
            {
                this.Hide();
                CurrrentModel.CallBack(ConfirmMode.Confirmed);
            }
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (CurrrentModel.CallBack != null)
            {
                this.Hide();
                CurrrentModel.CallBack(ConfirmMode.Cancelled);
            }
            this.Close();
        }


        public static void ShowMessage(ConfirmMessage model, Window owner, Point startPoint = default(Point))
        {
            Confirm win = new Confirm(model);
            
            if (owner != null && owner.Visibility == Visibility.Visible)
            {
                win.Owner = owner;
                if (startPoint != default(Point))
                {
                    startPoint = owner.PointToScreen(startPoint);
                    win.WindowStartupLocation = WindowStartupLocation.Manual;
                    win.Top = startPoint.Y;
                    win.Left = startPoint.X;
                }
            }
            win.ShowDialog();
        }
        public static void ShowMessage(string msg, Window owner, Action<ConfirmMode> callback, Point startPoint = default(Point))
        {
            ConfirmMessage model = new ConfirmMessage()
            {
                Message = msg,
                CallBack = callback
            };
            ShowMessage(model, owner, startPoint);
        }
    }
}
