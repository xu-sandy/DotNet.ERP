using Pharos.Wpf.ViewModelHelpers;
using System;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class ConfirmMessage : BaseViewModel
    {
        string title = "提示";
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                this.OnPropertyChanged(o => o.Title);
            }
        }

        string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                this.OnPropertyChanged(o => o.Message);
            }
        }

        public Action<ConfirmMode> CallBack { get; set; }
    }

    public enum ConfirmMode
    {
        Cancelled = 0,
        Confirmed = 1
    }
}
