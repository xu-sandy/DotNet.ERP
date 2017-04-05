﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Logic.Entity;
using Pharos.Utility;

namespace Pharos.Logic.BLL
{
    public class MemberLevelBLL
    {
        MemberLevelService _memberLevelService = new MemberLevelService();
        public OpResult CreateMemberLevel(MemberLevel level)
        {
            level.CompanyId = CommonService.CompanyId;
            if (level.Id == 0)
            {
                level.CreateDT = DateTime.Now;
                level.CreateUID = Sys.CurrentUser.UID;
                level.State = 0;//默认生效
                level.LevelSN = 0;
                var maxLevel = _memberLevelService.GetTop1MemberLevelByDTDesc();
                if (maxLevel != null)
                {
                    level.LevelSN = maxLevel.LevelSN + 1;
                }
                if (level.Discount <= 0)//没有折扣保存-1
                {
                    level.Discount = -1;
                }
                return _memberLevelService.CreateMemberLevel(level);
            }
            else {
                return _memberLevelService.UpdateMemberLevel(level);
            }
        }

        public object FindMemberLevelByCompanyId(out int count)
        {
            return _memberLevelService.FindMemberLevelByCompanyId(out count);
        }

        public OpResult UpdateMemberStateByIds(short state, string ids)
        {
            return _memberLevelService.UpdateMemberStateByIds(state, ids);
        }

        public IEnumerable<MemberLevel> GetAllMemberLevelInfo()
        {
            return _memberLevelService.GetAllMemberLevelInfo();
        }

        public MemberLevel GetMemberLevelById(int id)
        {
            return _memberLevelService.GetMemberLevelById(id);
        }
    }
}
