using Pharos.POS.Retailing.Models.ApiParams;
using Pharos.POS.Retailing.Models.ApiReturnResults;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class AddMember : BaseViewModel
    {
        public AddMember()
        {
            Task.Factory.StartNew(() =>
            {
                var _machineInfo = Global.MachineSettings.MachineInformations;
                AreaInfoParams _params = new AreaInfoParams()
                {
                    PID = 1,
                    StoreId = _machineInfo.StoreId,
                    MachineSn = _machineInfo.MachineSn,
                    CID = _machineInfo.CompanyId
                };
                var result = ApiManager.Post<AreaInfoParams, ApiRetrunResult<IEnumerable<AreaInfo>>>(@"api/GetAreas", _params);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (result.Code == "200")
                    {
                        Provinces = new AreaInfo[] { new AreaInfo() { Title = "请选择省份", AreaID = -1 } }.Concat(result.Result.OrderBy(o => o.OrderNum));
                        if (Provinces.Count() > 1)
                            ProvinceID = Provinces.FirstOrDefault(o => o.AreaID != -1).AreaID;
                    }
                    else
                    {
                        Provinces = new AreaInfo[] { new AreaInfo() { Title = "请选择省份" } };
                    }
                }));
            });

        }

        private string memberId;

        public string MemberId
        {
            get { return memberId; }
            set
            {
                var text = value.Trim();
                if (text.Length > 40)
                {
                    Toast.ShowMessage("会员编号超出！限制长度40个字符！", CurrentWindow);
                    return;
                }
                memberId = text;
                this.OnPropertyChanged(o => o.MemberId);
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                var text = value.Trim();
                if (text.Length > 40)
                {
                    Toast.ShowMessage("会员名称超出！限制长度50个字符！", CurrentWindow);
                    return;
                }
                name = text;
                this.OnPropertyChanged(o => o.Name);
            }
        }


        private bool sex = true;

        public bool Sex
        {
            get { return sex; }
            set
            {
                sex = value;
                this.OnPropertyChanged(o => o.Sex);
            }
        }

        private string phone;

        public string Phone
        {
            get { return phone; }
            set
            {

                var text = value.Trim();
                if (!Regex.Match(@"\d{11}", text).Success)
                {
                    if (text.Length > 11)
                    {
                        Toast.ShowMessage("手机号码格式不正确！", CurrentWindow);
                        return;
                    }
                }
                if (text.Length > 11)
                {
                    Toast.ShowMessage("手机号码超出！限制长度11个字符！", CurrentWindow);
                    return;
                }
                phone = text;
                this.OnPropertyChanged(o => o.Phone);
            }
        }

        private string zhifubao;

        public string ZhiFuBao
        {
            get { return zhifubao; }
            set
            {
                var text = value.Trim();
                if (text.Length > 40)
                {
                    Toast.ShowMessage("支付宝账号超出！限制长度50个字符！", CurrentWindow);
                    return;
                }
                zhifubao = text;
                this.OnPropertyChanged(o => o.ZhiFuBao);
            }
        }

        private string cardNo;
        public string CardNo
        {
            get { return cardNo; }
            set
            {
                var text = value.Trim();
                if (text.Length > 40)
                {
                    Toast.ShowMessage("会员卡号超出！限制长度50个字符！", CurrentWindow);
                    return;
                }
                cardNo = text;
                this.OnPropertyChanged(o => o.CardNo);
            }
        }

        private string weixin;

        public string WeiXin
        {
            get { return weixin; }
            set
            {
                var text = value.Trim();
                if (text.Length > 40)
                {
                    Toast.ShowMessage("微信账号超出！限制长度50个字符！", CurrentWindow);
                    return;
                }
                weixin = text;
                this.OnPropertyChanged(o => o.WeiXin);
            }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set
            {
                var text = value.Trim();
                if (text.Length > 40)
                {
                    Toast.ShowMessage("EMail账号超出！限制长度50个字符！", CurrentWindow);
                    return;
                }
                email = text;
                this.OnPropertyChanged(o => o.Email);

            }
        }


        private DateTime? birthday;

        public DateTime? Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                this.OnPropertyChanged(o => o.Birthday);
            }
        }

        private string partialAddress;

        public string PartialAddress
        {
            get { return partialAddress; }
            set
            {
                var text = value.Trim();
                if (text.Length > 1000)
                {
                    Toast.ShowMessage("EMail账号超出！限制长度1000个字符！", CurrentWindow);
                    return;
                }
                partialAddress = text;
                this.OnPropertyChanged(o => o.PartialAddress);
            }
        }
        IEnumerable<AreaInfo> provinces;
        public IEnumerable<AreaInfo> Provinces
        {
            get
            {
                return provinces;
            }
            set
            {
                provinces = value;
                this.OnPropertyChanged(o => o.Provinces);
                if (provinces.Count() > 0)
                    ProvinceID = provinces.FirstOrDefault().AreaID;

            }
        }
        private int provinceId;
        public int ProvinceID
        {
            get { return provinceId; }
            set
            {

                provinceId = value;
                Task.Factory.StartNew(() =>
                {

                    var _machineInfo = Global.MachineSettings.MachineInformations;
                    AreaInfoParams _params = new AreaInfoParams()
                    {
                        PID = value,
                        StoreId = _machineInfo.StoreId,
                        MachineSn = _machineInfo.MachineSn,
                        CID = _machineInfo.CompanyId
                    };
                    var result = ApiManager.Post<AreaInfoParams, ApiRetrunResult<IEnumerable<AreaInfo>>>(@"api/GetAreas", _params);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {

                        if (result.Code == "200")
                        {
                            Cities = new AreaInfo[] { new AreaInfo() { Title = "请选择城市", AreaID = -1 } }.Concat(result.Result.OrderBy(o => o.OrderNum));
                            if (Cities.Count() > 1)
                                CityID = Cities.FirstOrDefault(o => o.AreaID != -1).AreaID;
                        }
                        else
                        {
                            Cities = new AreaInfo[] { new AreaInfo() { Title = "请选择城市", AreaID = -1 } };
                        }
                    }));
                });
                this.OnPropertyChanged(o => o.ProvinceID);
            }
        }
        private int cityId;
        public int CityID
        {
            get { return cityId; }
            set
            {
                cityId = value;
                Task.Factory.StartNew(() =>
                {

                    var _machineInfo = Global.MachineSettings.MachineInformations;
                    AreaInfoParams _params = new AreaInfoParams()
                    {
                        PID = value,
                        StoreId = _machineInfo.StoreId,
                        MachineSn = _machineInfo.MachineSn,
                        CID = _machineInfo.CompanyId
                    };
                    var result = ApiManager.Post<AreaInfoParams, ApiRetrunResult<IEnumerable<AreaInfo>>>(@"api/GetAreas", _params);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {

                        if (result.Code == "200")
                        {
                            Areas = new AreaInfo[] { new AreaInfo() { Title = "请选择区县", AreaID = -1 } }.Concat(result.Result.OrderBy(o => o.OrderNum));
                            if (Areas.Count() > 1)
                                AreaID = Areas.FirstOrDefault(o => o.AreaID != -1).AreaID;
                        }
                        else
                        {
                            Areas = new AreaInfo[] { new AreaInfo() { Title = "请选择区县", AreaID = -1 } };
                        }
                    }));
                });
                this.OnPropertyChanged(o => o.CityID);
            }
        }
        private int areaId;
        public int AreaID
        {
            get { return areaId; }
            set
            {

                areaId = value;
                this.OnPropertyChanged(o => o.AreaID);
            }
        }

        IEnumerable<AreaInfo> cities;
        public IEnumerable<AreaInfo> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                this.OnPropertyChanged(o => o.Cities);
                if (cities.Count() > 0)
                {
                    CityID = cities.FirstOrDefault().AreaID;
                }
            }
        }

        IEnumerable<AreaInfo> areas;
        public IEnumerable<AreaInfo> Areas
        {
            get { return areas; }
            set
            {
                areas = value;
                this.OnPropertyChanged(o => o.Areas);
                if (areas.Count() > 0)
                    AreaID = areas.FirstOrDefault().AreaID;
            }
        }
        decimal yaJin;
        public decimal YaJin
        {
            get { return yaJin; }
            set
            {

                yaJin = value;
                this.OnPropertyChanged(o => o.YaJin);
            }
        }
        public ICommand ConfirmCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        if (this.ProvinceID <= 0)
                        {
                            ProvinceID = -1;
                            CityID = -1;
                            AreaID = -1;
                        }
                        var _machinesInfo = Global.MachineSettings.MachineInformations;
                        AddMemberParams _params = new AddMemberParams()
                        {
                            Address = this.PartialAddress,
                            Birthday = this.Birthday.HasValue ? Birthday.Value.ToString("yyyy-MM-dd") : null,
                            CurrentCityId = this.CityID,
                            Email = this.Email,
                            MemberNo = this.MemberId,
                            MobilePhone = this.Phone,
                            ProvinceID = this.ProvinceID,
                            CurrentCountyId = this.AreaID,
                            RealName = this.Name,
                            Sex = this.Sex,
                            Weixin = this.WeiXin,
                            Zhifubao = this.ZhiFuBao,
                            StoreId = _machinesInfo.StoreId,
                            MachineSn = _machinesInfo.MachineSn,
                            CID = _machinesInfo.CompanyId,
                            CardNo = CardNo,
                            YaJin = YaJin
                        };
                        var result = ApiManager.Post<AddMemberParams, ApiRetrunResult<object>>(@"api/AddMember", _params);
                        CurrentWindow.Dispatcher.Invoke(new Action(() =>
                        {
                            if (result.Code == "200")
                            {
                                Toast.ShowMessage("保存成功！", CurrentWindow);
                                CurrentWindow.Close();

                            }
                            else
                            {
                                Toast.ShowMessage(result.Message, CurrentWindow);
                            }
                        }));
                    });
                });
            }
        }
    }
}
