using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class SetWeightViewModel : BaseViewModel
    {
        public string Product { get; set; }

        public string Unit { get; set; }
        decimal weight;
        public decimal Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
                this.OnPropertyChanged(o => o.Weight);
            }
        }
        public string Barcode { get; set; }
        public GeneralCommand<object> SaveCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    if (Weight < 100 && Weight > 0)
                    {
                        PosViewModel.Current.Barcode = string.Format("27{0}{1}1", Barcode, ((int)(Weight * 1000)).ToString().PadLeft(5, '0'));
                        CurrentWindow.Close();
                    }
                    else
                    {
                        Toast.ShowMessage("重量须大于0，小于100！", CurrentWindow);
                    }
                });
            }
        }
    }
}
