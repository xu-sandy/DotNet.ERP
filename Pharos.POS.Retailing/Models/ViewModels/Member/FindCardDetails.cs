using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pharos.POS.Retailing.Models.ViewModels.Member
{
    public class FindCardDetails : BaseViewModel
    {
        /// <summary>
        /// 会员卡号
        /// </summary>
        private string cardNo;

        public string CardNo
        {
            get { return cardNo; }
            set
            {
                if (value == null)
                    value = "";
                cardNo = value.Trim();
                this.OnPropertyChanged(o => o.CardNo);
            }
        }

        private DateTime _Start = DateTime.Now.Date;
        public DateTime Start
        {
            get { return _Start; }
            set
            {

                _Start = value;
                this.OnPropertyChanged(o => o.Start);
            }
        }
        private DateTime _End = DateTime.Now.AddDays(1).Date;
        public DateTime End
        {
            get { return _End; }
            set
            {

                _End = value;
                this.OnPropertyChanged(o => o.End);
            }
        }

        private IEnumerable<StoredValueCardPayDetailsItem> details;
        public IEnumerable<StoredValueCardPayDetailsItem> Details
        {
            get { return details; }
            set
            {

                details = value;
                this.OnPropertyChanged(o => o.Details);
            }
        }

        public ICommand ConfirmCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    QueryModel.Current.IsQuery = true;
                    if (End - Start > new TimeSpan(10, 0, 0, 0))
                    {
                        Toast.ShowMessage("数据量太大，只能查询间隔10天的数据！", CurrentWindow);
                        return;
                    }
                    Task.Factory.StartNew(() =>
                    {
                        var _machinesInfo = Global.MachineSettings.MachineInformations;
                        FindCardDetailsParams _params = new FindCardDetailsParams()
                        {
                            End = End,
                            Start = Start,
                            CardNo = CardNo,
                            StoreId = _machinesInfo.StoreId,
                            MachineSn = _machinesInfo.MachineSn,
                            CID = _machinesInfo.CompanyId
                        };
                        var result = ApiManager.Post<FindCardDetailsParams, ApiRetrunResult<IEnumerable<StoredValueCardPayDetailsItem>>>(@"api/GetStoredValueCardPayDetails", _params);
                        if (result.Code == "200")
                        {
                            CurrentWindow.Dispatcher.Invoke(new Action(() =>
                            {
                                Details = result.Result;
                            }));
                            QueryModel.Current.IsQuery = false;
                        }
                        else
                        {
                            CurrentWindow.Dispatcher.Invoke(new Action(() =>
                            {
                                Toast.ShowMessage(result.Message, CurrentWindow);
                            }));
                            QueryModel.Current.IsQuery = false;
                        }
                    });

                });
            }
        }
    }
}
