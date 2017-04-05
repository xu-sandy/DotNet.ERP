﻿using Pharos.Wpf.ViewModelHelpers;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class HoykeyViewModel : BaseViewModel
    {
        public HoykeyViewModel()
        {
            //ButtonContent = "重置";
        }
        public string Name { get; set; }
        public string Title { get; set; }

        public bool EnableSet { get; set; }

        internal string _Keys;
        public string Keys
        {
            get { return _Keys.Replace("Left", "←").Replace("Right", "→").Replace("Up", "↑").Replace("Down", "↓").Replace("Control", "Ctrl").Replace("Plus", "+").Replace("Minus", "-"); }
            set
            {
                _Keys = value;
                this.OnPropertyChanged(o => o.Keys);
            }
        }

        //string _ButtonContent;
        //public string ButtonContent { get { return _ButtonContent; } set { _ButtonContent = value; this.OnPropertyChanged(o => o.ButtonContent); } }

        string _ButtonColor = "Gray";

        public string ButtonColor
        {
            get { return _ButtonColor; }
            set { _ButtonColor = value; this.OnPropertyChanged(o => o.ButtonColor); }
        }
    }
}
