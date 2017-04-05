using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Sys.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Store.Retailing.Models
{
    public class LogModel
    {
        public LogModel(SysLog entity)
        {
            CurrentLog = entity;
        }

        #region Property

        private SysLog CurrentLog { get; set; }

        public int Id
        {
            get { return CurrentLog.Id; }
            set { CurrentLog.Id = value; }
        }

        public byte Type
        {
            get { return CurrentLog.Type; }
            set { CurrentLog.Type = value; }
        }

        public string Summary
        {
            get { return CurrentLog.Summary; }
            set { CurrentLog.Summary = value; }
        }

        public string ClientIP
        {
            get { return CurrentLog.ClientIP; }
            set { CurrentLog.ClientIP = value; }
        }

        public string ServerName
        {
            get { return CurrentLog.ServerName; }
            set { CurrentLog.ServerName = value; }
        }

        public DateTime CreateDT
        {
            get { return CurrentLog.CreateDT; }
            set { CurrentLog.CreateDT = value; }
        }
        #endregion Property

    }
}