using Pharos.Wpf.Models;

namespace Pharos.POS.Retailing.Models.PosModels
{
   public enum ChangeInputMode
    {
       [EnumTitle(0,"原条码")]
       Old=0,
       [EnumTitle(1,"新条码")]
       New = 1
    }
}
