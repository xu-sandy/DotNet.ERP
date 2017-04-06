using Pharos.Wpf.ViewModelHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class QueryModel : BaseViewModel
    {
        public bool _IsQuery;
        public bool IsQuery
        {
            get { return _IsQuery; }
            set
            {
                _IsQuery = value;
                this.OnPropertyChanged(o => o.IsQuery);
            }
        }

        static QueryModel current = new QueryModel();

        public static QueryModel Current { get { return current; } }
    }
}
