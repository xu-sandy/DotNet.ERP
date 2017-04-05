using Pharos.POS.Retailing.Extensions;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.Controls;
using Pharos.Wpf.Extensions;
using System.Linq;
using System.Windows.Input;

namespace Pharos.POS.Retailing.ChildWin
{
    /// <summary>
    /// HuoDongXuanZhe.xaml 的交互逻辑
    /// </summary>
    public partial class HuoDongXuanZhe : DialogWindow02
    {
        public HuoDongXuanZhe()
        {
            InitializeComponent();
            model = new ActivityViewModel();
            this.ApplyBindings(this, model);
            this.InitDefualtSettings();
            this.PreviewKeyDown += HuoDongXuanZhe_PreviewKeyDown;
        }
        private ActivityViewModel model;
        void HuoDongXuanZhe_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D1:
                case Key.NumPad1:
                    if (model.Activities.Count() > 1)
                        model.Activities.ElementAt(1).SetCommand.Execute(null);
                    break;
                case Key.D2:
                case Key.NumPad2:
                    if (model.Activities.Count() > 2)
                        model.Activities.ElementAt(2).SetCommand.Execute(null);
                    break;
                case Key.D3:
                case Key.NumPad3:
                    if (model.Activities.Count() > 3)
                        model.Activities.ElementAt(3).SetCommand.Execute(null);
                    break;
                case Key.D4:
                case Key.NumPad4:
                    if (model.Activities.Count() > 4)
                        model.Activities.ElementAt(4).SetCommand.Execute(null);
                    break;
                case Key.D5:
                case Key.NumPad5:
                    if (model.Activities.Count() > 5)
                        model.Activities.ElementAt(5).SetCommand.Execute(null);
                    break;
                case Key.D6:
                case Key.NumPad6:
                    if (model.Activities.Count() > 6)
                        model.Activities.ElementAt(6).SetCommand.Execute(null);
                    break;
                case Key.D7:
                case Key.NumPad7:
                    if (model.Activities.Count() > 7)
                        model.Activities.ElementAt(7).SetCommand.Execute(null);
                    break;
                case Key.D8:
                case Key.NumPad8:
                    if (model.Activities.Count() > 8)
                        model.Activities.ElementAt(8).SetCommand.Execute(null);
                    break;
                case Key.D9:
                case Key.NumPad9:
                    if (model.Activities.Count() > 9)
                        model.Activities.ElementAt(9).SetCommand.Execute(null);
                    break;
                case Key.D0:
                case Key.NumPad0:
                    if (model.Activities.Count() > 0)
                        model.Activities.ElementAt(0).SetCommand.Execute(null);
                    break;

            }
        }
    }
}
