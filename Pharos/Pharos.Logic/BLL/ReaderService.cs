using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;

namespace Pharos.Logic.BLL
{
    public class ReaderService:BaseService<Reader>
    {
        /// <summary>
        /// 类型,1-公告,2-订单新审批,3-供应商新订单
        /// </summary>
        /// <param name="type"></param>
        /// <param name="UID"></param>
        /// <param name="ids">mainIds</param>
        public static void Add(short type,string UID,List<int> ids)
        {
            if(type==3)
            {
                if(!SupplierService.IsExist(o=>o.Id==UID))
                    throw new Pharos.Logic.ApiData.Mobile.Exceptions.MessageException("供应商编号不存在!");
            }
            var list = BaseService<Reader>.FindList(o => o.Type == type && ids.Contains(o.MainId));
            var readers = new List<Reader>();
            ids.ForEach(id =>
            {
                if (!list.Any(i => i.MainId == id && i.ReadCode == UID))
                    readers.Add(new Reader() { MainId = id, ReadCode = UID, Type = type });
            });
            if(readers.Any())
                BaseService<Reader>.AddRange(readers);
        }
    }
}
