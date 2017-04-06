using Pharos.POS.Retailing.Models.ViewModels.Member;
using Pharos.Wpf.Controls;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using Pharos.Wpf.Extensions;
using Pharos.Wpf.HotKeyHelper;
using System.Threading.Tasks;
using System.Threading;

namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// Member.xaml 的交互逻辑
    /// </summary>
    public partial class Member : DialogWindow02
    {
        MemberViewModel model;
        public Member()
        {
            InitializeComponent();
            model = new MemberViewModel();
            model.AddMember = new Models.ViewModels.AddMember();
            model.AddMember.CurrentBindingElement = addMemberTab;
            model.AddMember.CurrentWindow = this;
            model.FindMember = new Models.ViewModels.FindMember();
            model.FindMember.CurrentBindingElement = findMemberTab;
            model.FindMember.CurrentWindow = this;
            model.Details = new FindCardDetails();
            model.Details.CurrentBindingElement = findMemberTab;
            model.Details.CurrentWindow = this;
            model.Recharge = new Models.ViewModels.StoredValueCardRecharge();
            model.Recharge.CurrentBindingElement = findMemberTab;
            model.Recharge.CurrentWindow = this;
            this.ApplyBindings(this, model);
            this.ApplyHotKeyBindings();
            //this.InitDefualtSettings();
            this.PreviewKeyDown += Pharos.POS.Retailing.Extensions.WindowExtensions._this_PreviewKeyDown;
            this.PreviewKeyDown += Member_PreviewKeyDown;

        }
        public Member(int tabIndex = 0)
            : this()
        {
            InitTabIndex(tabIndex);
        }

        void Member_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((tabControl.SelectedIndex == 0 && e.KeyboardDevice.Modifiers != ModifierKeys.Control))
            {
                return;
            }
            switch (e.Key)
            {
                case Key.C:
                    InitTabIndex(0);
                    e.Handled = true;
                    break;
                case Key.Q:
                    InitTabIndex(1);
                    e.Handled = true;
                    break;
                case Key.D:
                    InitTabIndex(2);
                    e.Handled = true;
                    break;
                case Key.F2:
                    if (tabControl.SelectedIndex == 3 || tabControl.SelectedIndex == 4)
                    {
                        if (tabControl.SelectedIndex == 3)
                        {
                            rCardNo.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                            rCardNo.Focus();
                        }
                        if (tabControl.SelectedIndex == 4)
                        {
                            crCardNo.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                            crCardNo.Focus();
                        }
                        model.Recharge.QueryCommand.Execute(null);
                    }
                    break;
                case Key.F:
                    InitTabIndex(4);
                    e.Handled = true;
                    break;
                case Key.R:
                    InitTabIndex(3);
                    e.Handled = true;
                    break;
            }
        }

        private void InitTabIndex(int index)
        {
            TextBox focusControl = null;
            switch (index)
            {
                case 0:
                    tabControl.SelectedIndex = 0;
                    focusControl = txtCardNO;
                    break;
                case 1:
                    tabControl.SelectedIndex = 1;
                    focusControl = qPhone;
                    break;
                case 2:
                    tabControl.SelectedIndex = 2;
                    focusControl = dCardNo;
                    break;
                case 3:
                    tabControl.SelectedIndex = 3;
                    focusControl = rCardNo;
                    break;
                case 4:
                    tabControl.SelectedIndex = 4;
                    focusControl = crCardNo;
                    break;
            }
            if (focusControl != null)
            {
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(200);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        focusControl.Focus();

                    }));
                });
            }
        }

        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    ((Control)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    e.Handled = true;
                    break;
            }
        }

    }
}
