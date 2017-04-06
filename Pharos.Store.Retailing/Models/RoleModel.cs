using Pharos.Logic;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Store.Retailing.Models
{
    public class RoleModel
    {
        #region Constructor

        public RoleModel()
            : this(new SysRoles())
        {
        }

        public RoleModel(SysRoles role)
        {
            CurrentRole = role;
        }

        #endregion Constructor

        #region Property
        private SysRoles CurrentRole { get; set; }

        public int Id
        {
            get { return CurrentRole.Id; }
            set { CurrentRole.Id = value; }
        }

        public string Title
        {
            get { return CurrentRole.Title; }
            set { CurrentRole.Title = value; }
        }

        public string LimitsCode
        {
            get { return CurrentRole.LimitsIds; }
            set { CurrentRole.LimitsIds = value; }
        }

        public string Memo
        {
            get { return CurrentRole.Memo; }
            set { CurrentRole.Memo = value; }
        }

        public bool Status
        {
            get { return CurrentRole.Status; }
            set { CurrentRole.Status = value; }
        }
        #endregion Property

        #region Method
        public OpResult SaveChange()
        {
            OpResult re = new OpResult() { Successed = true };
            if (RoleInfoService.FindById(Id) == null)
            {
                re = RoleInfoService.Add(CurrentRole);
            }
            else
            {
                re = RoleInfoService.Update(CurrentRole);

            }
            return re;
        }
        #endregion Method

    }
}