using Pharos.POS.Retailing.Models.PosModels;
using Pharos.Wpf.ViewModelHelpers;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class PayItem : BaseViewModel
    {
        public PayItem()
        {
            IsOperatEnabled = true;
            this.OnPropertyChanged(o => o.IsOperatEnabled);
            //if (Mode == PayMode.CashCoupon && Amount < 0)
            //{
            //    Enable = false;
            //}
        }
        MachineInformations _machineInfo = Global.MachineSettings.MachineInformations;
        public string Title { get; set; }

        public bool Enable { get; set; }

        string _EnableIcon;
        public string EnableIcon
        {
            get
            {
                return _EnableIcon;
            }
            set
            {
                _EnableIcon = value;
                this.OnPropertyChanged(o => o.Icon);
            }
        }
        string _DisableIcon;
        public string DisableIcon
        {
            get
            {
                return _DisableIcon;
            }
            set
            {
                _DisableIcon = value;
                this.OnPropertyChanged(o => o.Icon);
            }
        }

        public string ApiCodes { get; set; }

        public PayMode Mode { get; set; }

        public decimal Amount { get; set; }
        public PayAction Action { get; set; }

        internal int Reason { get; set; }
        public ImageSource Icon
        {
            get
            {
                if (Enable)
                {
                    return new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, EnableIcon), UriKind.Absolute));
                }
                else
                {
                    return new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DisableIcon), UriKind.Absolute));

                }
            }
        }
        public string QrContent { get; set; }
        private ImageSource _RongHeDynamicQRCode;
        public ImageSource RongHeDynamicQRCode
        {
            get
            {
                return _RongHeDynamicQRCode;
            }
            set
            {
                _RongHeDynamicQRCode = value;
                this.OnPropertyChanged(o => o.RongHeDynamicQRCode);
            }
        }
        public bool IsOperatEnabled { get; set; }

        public char Key { get; set; }
    }
}
