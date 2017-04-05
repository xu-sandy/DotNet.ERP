﻿using System;
using System.IO;
using System.Web;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using Pharos.Utility.Caching;
using AX.CSF.Encrypt;
using Pharos.Logic.OMS.Entity;

namespace Pharos.Logic.OMS
{
    /// <summary>
    /// 系统授权许可
    /// </summary>
    public class SysAuthorize
    {
        /// <summary>
        /// 注册请求
        /// </summary>
        /// <param name="company">单位简称</param>
        /// <param name="fullCompany">单位全称</param>
        /// <returns>true:请求成功，false:请求失败</returns>
        public void Register(CompanyAuthorize obj)
        {
            //todo:
            //1. 获取该服务器机器码
            //obj.MachineSN = Machine.GetMAC;
            //obj.Status = 0;
            //2. 将机器码、单位名称 提交给OMS生成客户授权
            //var op= OMSCompanyAuthrizeBLL.Update(obj);
            var config = new Pharos.Utility.Config();
            config.SetAppSettings("CompanyId", obj.CID.ToString());
            
        }

        /// <summary>
        /// 验证激活
        /// </summary>
        /// <param name="sn">序列号</param>
        /// <returns>true:成功，false:失败</returns>
        public OpResult VerifyActivate(string sn)
        {
            var op = new OpResult();
            //DictRegister[SysCommonRules.CompanyId] = null;
            string message="";
            if (HasRegister(ref message, sn))
            {
                
            }
            //op.Successed = DictRegister[SysCommonRules.CompanyId].GetValueOrDefault();
            op.Message = message;
            return op;
        }

        public const string _SerialKey = "SerialNo";
        public CompanyAuthorize AnalysisSN(string sn)
        {
            if (!string.IsNullOrEmpty(sn))
            {
                try
                {
                    var text = DES.Decrypt(sn);
                    JObject json = JObject.Parse(text);
                    CompanyAuthorize company = new CompanyAuthorize();

                    company.CID = Convert.ToInt32(json["CID"]);
                    company.Title = Convert.ToString(json["Title"]);
                    company.Source = Convert.ToInt16(json["Source"]);
                    company.Way = Convert.ToInt16(json["Way"]);
                    company.BusinessMode = Convert.ToInt16(json["BusinessMode"]);

                    company.UserNum = Convert.ToInt16(json["UserNum"]);
                    company.StoreNum = Convert.ToInt16(json["StoreNum"]);
                    company.StoreProper = Convert.ToString(json["StoreProper"]);
                    company.AppProper = Convert.ToString(json["AppProper"]);
                    company.PosMinorDisp = Convert.ToString(json["PosMinorDisp"]);

                    company.OpenVersionId = Convert.ToInt16(json["OpenVersionId"]);
                    company.OpenScopeId = Convert.ToString(json["OpenScopeId"]);
                    company.EffectiveDT = Convert.ToString(json["EffectiveDT"]);
                    company.ValidityNum = Convert.ToInt16(json["ValidityNum"]);
                    company.ExpirationDT = Convert.ToString(json["ExpirationDT"]);

                    company.MachineSN = Convert.ToString(json["MachineSN"]);
                    company.SupperAccount = Convert.ToString(json["SupperAccount"]);
                    company.SupperPassword = Convert.ToString(json["SupperPassword"]);
                    return company;
                }
                catch { }
            }
            return null;
        }
        static Dictionary<int,bool?> DictRegister =new Dictionary<int,bool?>();
        public bool HasRegister(ref string message, string serialno = null)
        {
            return true;
        }
        bool ValidateProperty(string companyId,ref string message)
        {
            var omsurl = Config.GetAppSettings("omsurl") + "Authorization/GetCompany";
            var json = HttpClient.HttpGet(omsurl, "companyId=" + companyId);
            int comp=0;
            if (string.IsNullOrWhiteSpace(json) && int.TryParse(companyId,out comp))
            {
                message = "连接OMS管理平台失败!";
            }
            else
            {
                var company = json.ToObject<CompanyAuthorize>();
                if (company == null)
                {
                    message = "获取公司信息序列化失败!";
                }
                else if (company.CID == 0)
                {
                    message = "基础信息与注册不一致,请联系管理员!";
                }
                else if (company.Status !=1)
                {
                    message = "该公司处于非正常状态，请联系管理员!";
                }
                else if (company != null && DateTime.Parse(company.ExpirationDT) <= DateTime.Now)
                {
                    message = "已过使用期,请重新激活!";
                }
                else
                {
                    //DictRegister[SysCommonRules.CompanyId] = true;
                    //Sys.CurrentUser.Company = company;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 验证匹配字段
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool ValidateCompany(CompanyAuthorize auth, CompanyAuthorize source)
        {
            if (auth == null || source == null) return false;
            return (auth.Way == source.Way && auth.Title == source.Title && auth.MachineSN == source.MachineSN
                && auth.StoreNum == source.StoreNum && auth.Status == source.Status && auth.ValidityNum == source.ValidityNum);
        }
        /// <summary>
        /// 解析序列号
        /// </summary>
        public CompanyAuthorize GetSerialNO
        {
            get
            {
                string key = Config.GetAppSettings(_SerialKey);
                var companyId = 0;// SysCommonRules.CompanyId;
                CompanyAuthorize company = null;
                if (!string.IsNullOrWhiteSpace(key) && companyId>0)//离线
                {
                    company = AnalysisSN(key);
                }
                else if (companyId>0)
                {
                    //company = OMSCompanyAuthrizeBLL.GetByCode(companyId);
                }
                return company;
            }
        }
    }
}
