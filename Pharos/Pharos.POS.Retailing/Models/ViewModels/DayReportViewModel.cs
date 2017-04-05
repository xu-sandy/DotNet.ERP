﻿using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.POS.Retailing.Models.Printer;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class DayReportViewModel : BaseViewModel
    {
        public DayReportViewModel()
        {
            RangeSelectItem = new List<KeyValuePair<int, string>>();
            RangeSelectItem.Add(new KeyValuePair<int, string>(0, "本机日结"));
            RangeSelectItem.Add(new KeyValuePair<int, string>(1, "全店日结"));
            RangeSelectItem.Add(new KeyValuePair<int, string>(2, "本机月结"));
            RangeSelectItem.Add(new KeyValuePair<int, string>(3, "全店月结"));
            HoursSelectItem = new List<KeyValuePair<int, string>>();
            for (var i = 0; i <= 24; i++)
            {
                HoursSelectItem.Add(new KeyValuePair<int, string>(i, i + "时"));
            }
            MinuteSelectItem = new List<KeyValuePair<int, string>>();
            for (var i = 0; i < 60; i++)
            {
                MinuteSelectItem.Add(new KeyValuePair<int, string>(i, i + "分"));
            }
            //ModeSelectItem = new List<KeyValuePair<DayReportMode, string>>() 
            //{
            //    new KeyValuePair<DayReportMode,string>(DayReportMode.Day,"日结"),
            //    new KeyValuePair<DayReportMode,string>(DayReportMode.Month,"月结")
            //};
            Type = 0;
        }
        public List<KeyValuePair<int, string>> RangeSelectItem { get; set; }
        public List<KeyValuePair<int, string>> HoursSelectItem { get; set; }
        public List<KeyValuePair<int, string>> MinuteSelectItem { get; set; }




        int hour1 = 0;
        public int Hour1
        {
            get
            {
                return hour1;
            }
            set
            {
                hour1 = value;
                this.OnPropertyChanged(o => o.Hour1);
            }
        }
        int hour2 = 0;
        public int Hour2
        {
            get
            {
                return hour2;
            }
            set
            {
                hour2 = value;
                this.OnPropertyChanged(o => o.Hour2);
            }
        }
        int minute1 = 0;
        public int Minute1
        {
            get
            {
                return minute1;
            }
            set
            {
                minute1 = value;
                this.OnPropertyChanged(o => o.Minute1);
            }
        }
        int minute2 = 0;
        public int Minute2
        {
            get
            {
                return minute2;
            }
            set
            {
                minute2 = value;
                this.OnPropertyChanged(o => o.Minute2);
            }
        }
        int type;
        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                if (value < 2)
                {
                    SearchTime = DateTime.Now.Date;
                    Hour1 = 8;
                    Minute1 = 0;
                    Hour2 = 24;
                    Minute2 = 00;
                }
                else
                {
                    SearchTime = DateTime.Now.Date;
                    Hour1 = 0;
                    Minute1 = 0;
                    Hour2 = 0;
                    Minute2 = 0;
                }
                type = value;
                this.OnPropertyChanged(o => o.type);
            }
        }

        Range range = Range.Store;
        public Range Range
        {
            get
            {
                return range;
            }
            set
            {
                range = value;
                this.OnPropertyChanged(o => o.Range);
            }
        }

        //public List<KeyValuePair<DayReportMode, string>> ModeSelectItem { get; set; }

        DayReportMode mode = DayReportMode.Day;
        public DayReportMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
                this.OnPropertyChanged(o => o.Mode);
            }
        }


        DateTime searchTime = DateTime.Now;
        public DateTime SearchTime
        {
            get
            {
                return searchTime;
            }
            set
            {
                searchTime = value;
                this.OnPropertyChanged(o => o.SearchTime);
            }
        }

        IEnumerable<DayReportDetailItem> dayReportItems;
        public IEnumerable<DayReportDetailItem> DayReportItems
        {
            get
            {
                return dayReportItems;
            }
            set
            {
                dayReportItems = value;
                this.OnPropertyChanged(o => o.DayReportItems);
            }
        }

        IEnumerable<SalesmanDayReportResult> usersDayReportItems;

        public IEnumerable<SalesmanDayReportResult> UsersDayReportItems
        {
            get
            {
                return usersDayReportItems;
            }
            set
            {
                usersDayReportItems = value;
                this.OnPropertyChanged(o => o.UsersDayReportItems);
            }
        }
        public GeneralCommand<object> SearchCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    QueryModel.Current.IsQuery = true;
                    if (Hour1 > Hour2 || (Hour1 == Hour2 && Minute1 > Minute2))
                    {
                        Toast.ShowMessage("起始时间不能大于结束时间！", CurrentWindow);
                        return;
                    }
                    switch (Type)
                    {
                        case 0:
                            Range = ApiReturnResults.Range.Local;
                            Mode = DayReportMode.Day;
                            break;
                        case 1:
                            Range = ApiReturnResults.Range.Store;
                            Mode = DayReportMode.Day;
                            break;
                        case 2:
                            Range = ApiReturnResults.Range.Local;
                            Mode = DayReportMode.Month;
                            break;
                        case 3:
                            Range = ApiReturnResults.Range.Store;
                            Mode = DayReportMode.Month;
                            break;
                    }
                    DayReportItems = new List<DayReportDetailItem>();
                    UsersDayReportItems = new List<SalesmanDayReportResult>();
                    Task.Factory.StartNew(() =>
                    {
                        var _machinesInfo = Global.MachineSettings.MachineInformations;
                        //查询日结
                        DayReportParams _params = new DayReportParams()
                        {
                            StoreId = _machinesInfo.StoreId,
                            MachineSn = _machinesInfo.MachineSn,
                            Date = SearchTime.Date.AddHours(Hour1).AddMinutes(Minute1),
                            EndDate = SearchTime.Date.AddHours(Hour2).AddMinutes(Minute2),
                            Mode = Mode,
                            Range = Range,
                            CID = _machinesInfo.CompanyId
                        };
                        var result = ApiManager.Post<DayReportParams, ApiRetrunResult<DayReportResult>>(@"api/DayReport", _params);
                        CurrentWindow.Dispatcher.Invoke(new Action(() =>
                        {
                            if (result.Code == "200")
                            {
                                DayReportItems = result.Result.Summary;
                                UsersDayReportItems = result.Result.SalesmanRecords;
                            }
                            else
                            {
                                Toast.ShowMessage(result.Message, CurrentWindow);
                            }
                            QueryModel.Current.IsQuery = false;

                        }));
                    });
                });
            }
        }
        public GeneralCommand<object> PrintCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    if (usersDayReportItems == null || usersDayReportItems.Count() < 0)
                    {
                        Toast.ShowMessage("请先查询数据！", CurrentWindow);
                        return;
                    }
                    else
                    {
                        if (PosViewModel.Current.PrintStatus == PrintStatus.Open)
                        {
                            DayReportModel dayReport = new DayReportModel();
                            dayReport.TicketWidth = 30;
                            dayReport.StoreName = Global.MachineSettings.MachineInformations.StoreName;
                            dayReport.Title = (Mode == DayReportMode.Day ? "日结报表" : "月结报表");
                            dayReport.Title2 = (Range == ApiReturnResults.Range.Local ? "POS机号：" + Global.MachineSettings.MachineInformations.MachineSn : "全店");
                            dayReport.StockDateStr = (Mode == DayReportMode.Day ? "日结时间：" + SearchTime.ToString("yyyy-MM-dd") : "日结时间：" + SearchTime.ToString("yyyy-MM"));
                            dayReport.PrintDate = DateTime.Now;
                            List<TransactionItemModel> transactionItemList = new List<TransactionItemModel>();
                            foreach (var item in DayReportItems)
                            {
                                transactionItemList.Add(new TransactionItemModel(item.Project, item.Number, item.Amount));
                            }
                            dayReport.TransactionItemList = transactionItemList;

                            List<EmployeeModel> employeeList = new List<EmployeeModel>();
                            foreach (var item in UsersDayReportItems)
                            {
                                EmployeeModel employee1 = new EmployeeModel();
                                employee1.EmployeeSN = item.UserCode;
                                employee1.Name = item.Salesman;
                                employee1.EmployeeTransactionItems = new List<TransactionItemModel>();

                                employee1.EmployeeTransactionItems.Add(new TransactionItemModel(item.Sale.SaleInfo.Project, item.Sale.SaleInfo.Number, item.Sale.SaleInfo.Amount, item.Sale.PayWay.ToDictionary(o => o.Title, o => o.Amount)));
                                foreach (var node in item.Other)
                                {
                                    var paywayDetails = new Dictionary<string, decimal>();

                                    if (node.PayWay != null && node.PayWay.Count > 0)
                                    {
                                        foreach (var itm in node.PayWay)
                                        {
                                            if (itm.Amount != 0m)
                                            {
                                                paywayDetails.Add(itm.Title, itm.Amount);
                                            }
                                        }
                                    }

                                    employee1.EmployeeTransactionItems.Add(new TransactionItemModel(node.Project, node.Number, node.Amount, paywayDetails));
                                }
                                employee1.EmployeeTransactionItems.Insert(employee1.EmployeeTransactionItems.Count - 1, new TransactionItemModel("结余现金", item.Cash));
                                employee1.BeginTime = item.StartTime;
                                employee1.EndTime = item.EndTime;
                                employeeList.Add(employee1);
                            }
                            dayReport.EmployeeList = employeeList;
                            PrintModelHelper helper = new PrintModelHelper();
                            string printStr = helper.GetDailyReportStr(dayReport);
                            PrintHelper.Print(printStr, null, true);
                        }
                    }
                });
            }
        }
    }

}
