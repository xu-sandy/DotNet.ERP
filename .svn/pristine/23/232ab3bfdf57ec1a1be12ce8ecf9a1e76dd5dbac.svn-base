using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.BLL.LocalServices.DataSync;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using Pharos.Logic.BLL.DataSynchronism.Dtos;
using Pharos.Sys.Entity;
using Pharos.Logic.BLL.DataSynchronism.Daos;
using Pharos.Logic.Entity;
namespace Pharos.Logic.BLL
{
    public class FileHandlerService
    {
        public static string Handler(short methodItem, string storeId, string exportItem, string importItem,bool zipFile=true)
        {
            string msg = "";
            try
            {
                var info = new UpdateFormData();
                Dictionary<string, string> filenames = new Dictionary<string, string>();
                if (methodItem == 1 && !exportItem.IsNullOrEmpty())
                {
                    info.StoreId = storeId;
                    info.Mode = DataSyncMode.FromServerDownload;
                    var zipfileName ="未知";
                    switch (exportItem)
                    {
                        case "Notice":
                            info.Datas["Pharos.Logic.LocalEntity.Notice"] = new List<NoticeForLocal>();
                            filenames["Pharos.Logic.LocalEntity.Notice"] = "公告信息";
                            zipfileName = "公告信息";
                            break;
                        case "ApiLibrary":
                            info.Datas["Pharos.Logic.LocalEntity.ApiLibrary"] = new List<ApiLibraryForLocal>();
                            filenames["Pharos.Logic.LocalEntity.ApiLibrary"] = "接口信息";
                            zipfileName =  "接口信息";

                            break;
                        case "SysStoreUserInfo":
                            info.Datas["Pharos.Logic.LocalEntity.SysStoreUserInfo"] = new List<SysStoreUserInfoForLocal>();
                            filenames["Pharos.Logic.LocalEntity.SysStoreUserInfo"] = "用户信息";
                            zipfileName =  "用户信息";

                            break;
                        case "Members":
                            info.Datas["Pharos.Logic.LocalEntity.Members"] = new List<MembersForLocal>();
                            filenames["Pharos.Logic.LocalEntity.Members"] = "会员信息";
                            zipfileName =  "会员信息";

                            break;
                        case "Product":
                            info.Datas["Pharos.Logic.LocalEntity.ProductBrand"] = new List<ProductBrandForLocal>();
                            info.Datas["Pharos.Logic.LocalEntity.ProductCategory"] = new List<ProductCategoryForLocal>();
                            info.Datas["Pharos.Logic.LocalEntity.SysDataDictionary"] = new List<SysDataDictionaryForLocal>();
                            info.Datas["Pharos.Logic.LocalEntity.ProductInfo"] = new List<ProductInfoForLocal>();
                        //    info.Datas["Pharos.Logic.LocalEntity.Commodity"] = new List<CommodityForLocal>();
                            filenames["Pharos.Logic.LocalEntity.ProductBrand"] = "品牌信息";
                            filenames["Pharos.Logic.LocalEntity.ProductCategory"] = "类别信息";
                            filenames["Pharos.Logic.LocalEntity.SysDataDictionary"] = "字典信息";
                            filenames["Pharos.Logic.LocalEntity.ProductInfo"] = "商品信息";
                            filenames["Pharos.Logic.LocalEntity.Commodity"] = "库存信息";
                            zipfileName =  "商品信息";

                            break;
                        case "Product2":
                            info.Datas["Pharos.Logic.LocalEntity.ProductInfo2"] = new List<ProductInfoForLocal>();
                            filenames["Pharos.Logic.LocalEntity.ProductInfo2"] = "商品称重信息";
                            zipfileName =  "商品称重信息";

                            break;
                        case "Product3":
                            info.Datas["Pharos.Logic.LocalEntity.ProductInfo"] = new List<ProductInfoForLocal>();
                            filenames["Pharos.Logic.LocalEntity.ProductInfo"] = "商品信息";
                            zipfileName = "商品信息";
                            break;
                        case "Promotion":
                            info.Datas["Pharos.Logic.LocalEntity.CommodityPromotion"] = new List<CommodityPromotionForLocal>();
                            info.Datas["Pharos.Logic.LocalEntity.Bundling"] = new List<BundlingForLocal>();
                            info.Datas["Pharos.Logic.LocalEntity.BundlingList"] = new List<BundlingListForLocal>();
                            info.Datas["Pharos.Logic.LocalEntity.CommodityDiscount"] = new List<CommodityDiscountForLocal>();
                            info.Datas["Pharos.Logic.LocalEntity.FreeGiftPurchase"] = new List<FreeGiftPurchaseForLocal>();
                            info.Datas["Pharos.Logic.LocalEntity.FreeGiftPurchaseList"] = new List<FreeGiftPurchaseListForLocal>();
                            info.Datas["Pharos.Logic.LocalEntity.PromotionBlend"] = new List<PromotionBlendForLocal>();
                            info.Datas["Pharos.Logic.LocalEntity.PromotionBlendList"] = new List<PromotionBlendListForLocal>();
                            filenames["Pharos.Logic.LocalEntity.CommodityPromotion"] = "主促销信息";
                            filenames["Pharos.Logic.LocalEntity.Bundling"] = "捆绑信息";
                            filenames["Pharos.Logic.LocalEntity.BundlingList"] = "捆绑商品信息";
                            filenames["Pharos.Logic.LocalEntity.CommodityDiscount"] = "商品折扣信息";
                            filenames["Pharos.Logic.LocalEntity.FreeGiftPurchase"] = "买赠信息";
                            filenames["Pharos.Logic.LocalEntity.FreeGiftPurchaseList"] = "买赠商品信息";
                            filenames["Pharos.Logic.LocalEntity.PromotionBlend"] = "组合信息";
                            filenames["Pharos.Logic.LocalEntity.PromotionBlendList"] = "组合商品信息";
                            zipfileName =  "促销信息";

                            break;
                        default:
                            break;
                    }
                    var data = DataSyncContext.ExportAll(info);
                    string relativePath = "";
                    var root = Sys.SysConstPool.SaveAttachPath(ref relativePath, "temp");
                    bool hasFile = false;
                    var maxrecord = System.Web.HttpContext.Current.Request["MaxRecord"].IsNullOrEmpty() ? 0 :
                        int.Parse(System.Web.HttpContext.Current.Request["MaxRecord"]);

                    foreach (var de in data.Datas)
                    {
                        if (!de.Value.Any()) continue;
                        var fileName = filenames[de.Key];
                        if (fileName == "商品称重信息")
                        {
                            var list = de.Value;
                            var count =maxrecord % list.Count() == 0 ? list.Count() / maxrecord : list.Count() / maxrecord+1;
                            for (int i = 0; i < count; i++)
                            {
                                list = de.Value.Skip(i * maxrecord).Take(maxrecord);
                                var fn=fileName+(i+1)+ ".txp";
                                CreateTxt(Path.Combine(root, fn), list, storeId);
                                filenames[fn] = Path.Combine(root, fn);
                            }
                            if (count == 0)
                            {
                                fileName = fileName + ".txp";
                                CreateTxt(Path.Combine(root, fileName), list, storeId);
                            }
                        }
                        else
                        {
                            fileName = fileName + ".xls";
                            Pharos.Utility.ExportExcelForCS.ToExcel<object>(fileName, root, de.Value);
                        }
                        filenames[de.Key] = Path.Combine(root, fileName);
                        hasFile = true;
                    }
                    if (hasFile)
                    {
                        byte[] buffer;
                        if (zipFile)
                        {
                            Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(Encoding.UTF8);
                            foreach (var de in filenames)
                            {
                                if (!File.Exists(de.Value))
                                    continue;
                                zip.AddFile(de.Value, "");
                            }
                            var stream = new MemoryStream();
                            zip.Save(stream);
                            zipfileName=zipfileName + ".zip";
                            buffer = stream.GetBuffer();
                        }
                        else
                        {
                            var path = filenames.FirstOrDefault().Value;
                            var stream = new FileStream(path, FileMode.Open);
                            zipfileName = zipfileName + Path.GetExtension(path);
                            buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);
                        }
                        
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ClearHeaders();
                        HttpContext.Current.Response.ContentType = "application/octet-stream";
                        if (HttpContext.Current.Request.Browser.Browser.Equals("InternetExplorer", StringComparison.CurrentCultureIgnoreCase))
                        {
                            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" +HttpUtility.UrlEncode(zipfileName));
                        }
                        else
                        {
                            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;fileName=" + zipfileName);
                        }
                        HttpContext.Current.Response.BinaryWrite(buffer);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.End();
                    }
                    else
                        msg = "该项暂无数据";
                }
                else if (methodItem == 2 && !importItem.IsNullOrEmpty())
                {
                    var list = new List<dynamic>();
                    for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                    {
                        var file = HttpContext.Current.Request.Files[i];
                        if (file.ContentLength <= 0) continue;
                        var workbook = ExportExcelForCS.InitWorkbook(file.FileName, file.InputStream);
                        var title = ExportExcelForCS.GetHeader(file.FileName, file.InputStream, workbook);
                        if (title.IsNullOrEmpty()) continue;
                        try
                        {
                            switch (importItem)
                            {
                                case "PosIncomePayout":
                                    list.AddRange(ExportExcelForCS.ReadListFromStream(new PosIncomePayoutForLocal(), workbook, false).Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<PosIncomePayout>(o)));
                                    break;
                                case "SalesReturns":

                                    //var key =LocalDataSyncContext.TableNames.FirstOrDefault(p => p.Value == title).Key;
                                    //var result = ExportExcelForCS.ReadListFromStream(LocalDataSyncContext.Entities[key], workbook, false);
                                    if (title.Contains("售后退换"))
                                    {
                                        list.AddRange(ExportExcelForCS.ReadListFromStream(new SalesReturnsForLocal(), workbook, false).Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<SalesReturns>(o)));
                                    }
                                    if (title.Contains("换货明细"))
                                    {
                                        list.AddRange(ExportExcelForCS.ReadListFromStream(new SalesReturnsDetailedForLocal(), workbook, false).Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<SalesReturnsDetailed>(o)));
                                    }
                                    break;
                                case "SaleOrders":
                                    if (title.Contains("销售单"))
                                    {
                                        list.AddRange(ExportExcelForCS.ReadListFromStream(new SaleOrdersForLocal(), workbook, false).Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<SaleOrders>(o)));
                                    }
                                    if (title.Contains("销售明细"))
                                    {
                                        list.AddRange(ExportExcelForCS.ReadListFromStream(new SaleDetailForLocal(), workbook, false).Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<SaleDetail>(o)));
                                    }
                                    if (title.Contains("消费支付"))
                                    {
                                        list.AddRange(ExportExcelForCS.ReadListFromStream(new ConsumptionPaymentForLocal(), workbook, false).Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<ConsumptionPayment>(o)));
                                    }
                                    break;
                                case "SysStoreUserInfo":
                                    list.AddRange(ExportExcelForCS.ReadListFromStream(new SysStoreUserInfoForLocal(), workbook, false).Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<SysStoreUserInfo>(o)));
                                    break;
                                case "MemberIntegral":
                                    list.AddRange(ExportExcelForCS.ReadListFromStream(new MemberIntegralForLocal(), workbook, false).Select(o => Pharos.Logic.Entity.BaseEntityExtension.InitEntity<MemberIntegral>(o)));
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch(Exception ex)
                        {
                            throw new Exception(title + "," + ex.Message);
                        }
                    }
                    foreach (var obj in list)
                    {
                        if (obj is SysStoreUserInfo)
                        {
                            var user = obj as SysStoreUserInfo;
                            var u = BaseService<SysStoreUserInfo>.Find(o => o.UserCode == user.UserCode);
                            if (u == null) continue;
                            u.LoginDT = user.LoginDT;
                        }
                        else
                            Save(obj, false);
                    }
                    if (list.Any())
                    {
                        Save(list[0], true);
                        msg = "导入成功";
                    }
                    else
                        msg = "请选择导入文件";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                new Sys.LogEngine().WriteError(ex);
            }
            return msg;
        }
        static bool Save<TEntity>(TEntity obj, bool save) where TEntity : class
        {
            if (save) BaseService<TEntity>.CurrentRepository._context.SaveChanges();
            BaseService<TEntity>.CurrentRepository.Add(obj, false);
            return true;
        }
        static void CreateTxt(string fullName,IEnumerable<object> list,string storeId)
        {
            if (!list.Any()) return;
            List<SysDataDictionary> dataDictionaryList = new List<SysDataDictionary>();
            dataDictionaryList = BaseService<SysDataDictionary>.CurrentRepository.QueryEntity.ToList();
            System.Text.StringBuilder sb = new StringBuilder();
            foreach(var obj in list)
            {
                var pro = obj as ProductRecord;
                sb.Append("0");//PLU No.	流水号
                sb.Append("\t");
                sb.Append(pro.Title);//Name	品名
                sb.Append("\t");
                sb.Append(pro.ProductCode);//LFCode	生鲜码
                sb.Append("\t");
                sb.Append(pro.ProductCode);//Code	货号
                sb.Append("\t");
                sb.Append("79");//Barcode Type	条码类型
                sb.Append("\t");
                sb.Append((pro.SysPrice * 100).ToString("f0"));//Unit Price	单价
                sb.Append("\t");
                sb.Append(GetWeightUnitBySubUnitId(pro.SubUnitId,dataDictionaryList));//Weight Unit	称重单位
                sb.Append("\t");
                sb.Append(storeId.PadLeft(2, '0'));//Deptment	部门
                sb.Append("\t");
                sb.Append("0");//Tare	皮重
                sb.Append("\t");
                sb.Append("0");//Shelf Time	保存期
                sb.Append("\t");
                sb.Append("0");//Package Type	包装类型
                sb.Append("\t");
                sb.Append("0");//Package Weight	包装重量
                sb.Append("\t");
                sb.Append("0");//Package Tolerance	包装误差
                sb.Append("\t");
                sb.Append("0");//Message1	信息1
                sb.Append("\t");
                sb.Append("0");//Message2	信息2
                sb.Append("\t");
                sb.Append("0");//Account	会计信息
                sb.Append("\t");
                sb.Append("0");//Multi Label	多标签
                sb.Append("\t");
                sb.Append("0");//Rebate	单价折扣
                sb.Append("\t");
                sb.AppendLine("0");//PCS Type	数量类型
            }
            using(var sw=new StreamWriter(fullName,false,Encoding.GetEncoding("gb2312")))
            {
                sw.Write(sb.ToString());
                sw.Close();
            }
        }

        private static string GetWeightUnitBySubUnitId(int subUnitId, List<SysDataDictionary> dataDictionaryList)
        {
            var result = "4";
            var unit = dataDictionaryList.FirstOrDefault(o => o.DicSN == subUnitId);
            if (unit != null)
            {
                var unitTitle = unit.Title.Trim().ToLower();
                switch (unitTitle) {
                    case "KG": result = "4";
                        break;
                    case "公斤": result = "4";
                        break;
                    case "500G": result = "7";
                        break;
                    case "斤": result = "7";
                        break;
                    case "G": result = "1";
                        break;
                    case "克": result = "1";
                        break;
                }
            }

            return result;
        }
    }
}
