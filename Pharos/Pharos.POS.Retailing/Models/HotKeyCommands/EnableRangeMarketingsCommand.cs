﻿using Pharos.POS.Retailing.Models.ApiParams;
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
    public class EnableRangeMarketingsCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((win) =>
                {
                    Task.Factory.StartNew(() =>//POST /api/SaleOrderEnableMarketing
                    {
                        PosViewModel.Current.EnableRangeMarketings = !PosViewModel.Current.EnableRangeMarketings;
                        SaleEnableMarketingParams _params = new SaleEnableMarketingParams()
                        {
                            CID = Global.MachineSettings.MachineInformations.CompanyId,
                            EnableRangeMarketings = PosViewModel.Current.EnableRangeMarketings,
                            MachineSn = Global.MachineSettings.MachineInformations.MachineSn,
                            StoreId = Global.MachineSettings.MachineInformations.StoreId
                        };

                        var result = ApiManager.Post<SaleEnableMarketingParams, ApiRetrunResult<ApiSaleReturn, ProductInfo>>(@"api/SaleOrderEnableMarketing", _params);
                        if (result.Code == "200")
                        {
                            win.Dispatcher.Invoke(new Action(() =>
                            {
                                PosViewModel.Current.OrderList = result.Result.BuyList;
                                PosViewModel.Current.Preferential = result.Result.Statistics.Preferential;
                                PosViewModel.Current.ManJianPreferential = result.Result.Statistics.ManJianPreferential;
                                PosViewModel.Current.Receivable = result.Result.Statistics.Receivable;
                                PosViewModel.Current.Num = result.Result.Statistics.Num;
                                PosViewModel.Current.OrderSn = result.Result.Statistics.OrderSn;
                                PosViewModel.Current.GetPreferentialTitle(result.Result.Statistics.ManYuanPreferential, result.Result.Statistics.ZuHePreferential, result.Result.Statistics.ManJianPreferential);

                            }));
                            //重置销售状态
                            PosViewModel.Current.PosStatus = Models.PosStatus.Normal;
                        }
                        else
                        {
                            win.Dispatcher.Invoke(new Action(() =>
                            {
                                Toast.ShowMessage(result.Message, win);
                            }));
                        }

                    });
                });
            }
        }
    }
}
