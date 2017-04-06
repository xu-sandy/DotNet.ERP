﻿using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.BLL;
using Pharos.Utility.Helpers;
using System.Web.Http;
using System.Web.WebPages.Html;

namespace QCT.Pay.Admin.Controllers
{
    public class OuterApiController : ApiController
    {
        [Ninject.Inject]
        ProductService ProductService { get; set; }
        [Ninject.Inject]
        CompAuthorService CompAuthorService { get; set; }
        [Ninject.Inject]
        TradersService TradersService { get; set; }
        /// <summary>
        /// 对外提供商品信息
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns></returns>
        [HttpGet]
        public VwProduct GetProduct(string barcode)
        {
            var obj= ProductService.GetProductByBarcode(barcode);
            return obj;
        }
        /// <summary>
        /// 向商品中心添加信息
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        [HttpPost]
        public int PostProduct([FromBody]List<VwProduct> products)
        {
            return Convert.ToInt32(ProductService.AddProduct(products));
        }
        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public CompanyAuthorize GetCompany([FromBody]CompanyAuthorize obj)
        {
            if (obj.Title.IsNullOrEmpty())
                return CompAuthorService.GetOneTitleByCID(obj.CID.GetValueOrDefault());
            else
            {
                if(obj!=null)
                {
                    var source = CompAuthorService.GetOneTitleByCID(obj.CID.GetValueOrDefault());
                    if (!CompAuthorService.ValidateCompany(obj, source))
                        source.CID = 0;
                    return source;
                }
            }
            return null;
        }
        /// <summary>
        /// 重新注册商户信息
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost]
        public int RegisterAgain([FromBody]CompanyAuthorize company)
        {
            return Convert.ToInt32(CompAuthorService.RegisterAgain(company));
        }
        /// <summary>
        /// 注册商户信息
        /// </summary>
        /// <param name="trader">{}</param>
        /// <param name="orderList"></param>
        /// <param name="way"></param>
        /// <param name="openVersionId"></param>
        /// <param name="storeProper"></param>
        /// <param name="posMinorDisp"></param>
        /// <param name="appProper"></param>
        /// <param name="machine"></param>
        /// <returns></returns>
        [HttpPost]
        public int Register([FromBody]Traders trader, string orderList, short way, short openVersionId, string storeProper, string posMinorDisp, string appProper, string machine)
        {
            //throw new Exception("test");
            //trader.Source = 2;
            trader.TrackStautsId = 206;
            var op = TradersService.Save(trader, orderList, way, openVersionId, storeProper, posMinorDisp, appProper, machine);
            return Convert.ToInt32(op.Successed);
        }
        /// <summary>
        /// 获取注册时数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetRegisterData(int companyId)
        {
            var traderTypes = TradersService.getTraderTypeList().Select(o => new SelectListItem() { Value = o.TraderTypeId.ToString(), Text = o.Title });
            var modes =TradersService.getDataList().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title });
            var busines = TradersService.getBusinessList().Select(o => new SelectListItem() { Value = o.ById, Text = o.Title });
            var tracks = TradersService.getDataList(205).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title });
            var orderList = TradersService.getOrderList(companyId);
            var trader = TradersService.GetOneByCID(companyId);
            return new { traderTypes = traderTypes, modes = modes, busines = busines, tracks = tracks, orderList = orderList, trader = trader };
        }
        /// <summary>
        /// 获取系列号
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSerialNo([FromBody]string companyId)
        {
            var obj= CompAuthorService.GetOneTitleByCID(int.Parse(companyId));
            if (obj != null)
            {
                var id = obj.Id.ToString();
                var dicts = CompAuthorService.GenerateSerial(id);
                if (dicts.ContainsKey(id)) return dicts[id];
            }
            return "";
        }
        /// <summary>
        /// 获取商户号
        /// </summary>
        /// <param name="name">子域名</param>
        /// <returns>-1-url为空，0-没有配置子域名</returns>
        [HttpGet]
        public int GetCIDByRealm(string name)
        {
            return CompAuthorService.GetCompanyIdByRealm(name);
        }
    }
}