using Pharos.POS.Retailing.ChildWin;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Pharos.POS.Retailing.Models.PosModels;
using System.Windows.Controls;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class DaoGouYuanCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    int pageSource = 0;//poswindow

                    if (win is TuiHuanHuo)
                    {
                        var tpage = win as IBarcodeControl;
                        if (tpage != null)
                        {
                            var txt = tpage.CurrentIInputElement as TextBox;
                            if (txt != null && txt.Name.Contains("Change"))
                            {
                                pageSource = 1;
                            }
                            else if (txt != null && txt.Name.Contains("Refund") && !txt.Name.Contains("RefundAll"))
                            {
                                pageSource = 2;
                            }
                            else
                            {
                                return;
                            }

                        }
                        else
                        {
                            return;
                        }
                    }

                    DaoGouYuan page = new DaoGouYuan(pageSource);
                    page.Owner = win;
                    page.ShowDialog();
                });
            }
        }
    }
}
