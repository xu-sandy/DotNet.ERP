using Pharos.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using System.Windows.Shapes;

namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// AddMember.xaml 的交互逻辑
    /// </summary>
    public partial class AddMember : DialogWindow02
    {
        public AddMember()
        {

            InitializeComponent();
            this.InitDefualtSettings();
         //   var model =new{ AddMember =new Pharos.POS.Retailing.Models.ViewModels.AddMember()};
          //  this.ApplyBindings(this, model);
            dt.PreviewKeyDown += dt_PreviewKeyDown;
            this.Loaded += AddMember_Loaded;
        }

        void dt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtPhone.Focus();
                e.Handled = true;
            }
        }

        void AddMember_Loaded(object sender, RoutedEventArgs e)
        {
            txtMemberID.Focus();
        }
    }
}
