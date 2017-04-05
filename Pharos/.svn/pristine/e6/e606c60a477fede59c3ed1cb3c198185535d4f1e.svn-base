using Pharos.Logic.LocalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.BLL.LocalServices
{
    public class SysDataDictionaryLocalService : BaseLocalService<SysDataDictionary>
    {
        public static string GetUnitDict(int dicSN)
        {
            var dict = CurrentRepository.Find(o => o.DicSN == dicSN);
            if (dict != null)
                return dict.Title;
            return "未知单位";
        }

        public static int GetSalesClassifyId(string title)
        {
            var dict = CurrentRepository.Find(o => o.Title == title);
            return dict == null ? -1 : dict.DicSN;
        }
    }
}
