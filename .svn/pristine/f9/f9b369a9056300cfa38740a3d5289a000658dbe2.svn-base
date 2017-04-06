using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.POS.Retailing.Models.ViewModels;
using Pharos.Wpf.HotKeyHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class ChongDaYinCommand : IHotKeyCommand
    {
        static bool isRun = false;
        static object lockobj = new object();
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    if (isRun) return;
                    lock (lockobj)
                    {
                        isRun = true;
                        try
                        {
                            win.Dispatcher.Invoke(new Action(() =>
                            {
                                var model = new FindBills();
                                model.CurrentWindow = win;
                                model.SearchTime = DateTime.Now.Date;
                                model.Callback = () =>
                                {
                                    if (model.OrderItems != null)
                                    {
                                        var order = model.OrderItems.FirstOrDefault();
                                        if (order != null)
                                        {
                                            order.Print.Execute(null);
                                            return;
                                        }
                                        else
                                        {
                                            goto Error;
                                        }
                                    }
                                    else
                                    {
                                        goto Error;
                                    }
                                Error:
                                    {
                                        Application.Current.Dispatcher.Invoke(new Action(() =>
                                        {
                                            Toast.ShowMessage("无订单数据，不能打印！", win);
                                        }));
                                    }
                                };
                                model.SearchCommand.Execute(Range.Local);

                            }));
                        }
                        catch (Exception ex)
                        {
                            isRun = false;

                        }
                        isRun = false;
                    }
                });
            }
        }
    }
}