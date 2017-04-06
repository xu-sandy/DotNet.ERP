using Pharos.POS.Retailing.Models.PosModels;
using Pharos.Wpf.ViewModelHelpers;
using System.Windows.Input;

namespace Pharos.POS.Retailing.Models.ViewModels
{
    public class GotoViewModel : BaseViewModel
    {
        public int index;

        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                this.OnPropertyChanged(o => o.Index);
            }
        }

        public ICommand GotoCommand
        {
            get
            {
                return new GeneralCommand<object>((o1, o2) =>
                {
                    if (CurrentWindow.Owner != null && CurrentWindow.Owner is IPosDataGrid)
                    {
                        var dg = CurrentWindow.Owner as IPosDataGrid;
                        var index = Index - 1;
                        if (index >= 0)
                        {
                            dg.CurrentGrid.SelectedIndex = index;
                            if (dg.CurrentGrid.SelectedItem != null)
                                dg.CurrentGrid.ScrollIntoView(dg.CurrentGrid.SelectedItem);
                        }
                    }
                    CurrentWindow.Close();
                });
            }

        }
    }
}
