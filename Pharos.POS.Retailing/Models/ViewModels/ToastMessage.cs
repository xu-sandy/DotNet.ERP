using Pharos.Wpf.ViewModelHelpers;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class ToastMessage : BaseViewModel
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

        int seconds = 3;
        public int Seconds
        {
            get
            {
                return seconds;
            }
            set
            {
                if (value == 0)
                {
                    CurrentWindow.Close();
                }
                seconds = value;
                this.OnPropertyChanged(o => o.Seconds);
            }
        }

    }
}
