using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Pharos.Logic.Entity;
using Pharos.Utility;
using Pharos.Utility.Helpers;

namespace Pharos.Logic.BLL
{
    public class ImportSetService : BaseService<ImportSet>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="httpFiles"></param>
        /// <param name="fieldName"></param>
        /// <param name="columnName"></param>
        /// <param name="fieldCols">字段和对应列</param>
        /// <param name="dt">导入表数据</param>
        /// <returns></returns>
        public static OpResult ImportSet(ImportSet obj, System.Web.HttpFileCollectionBase httpFiles, string fieldName, string columnName, ref Dictionary<string, char> fieldCols, ref System.Data.DataTable dt)
        {
            var op = new OpResult();

            if (httpFiles.Count <= 0 || httpFiles[0].ContentLength <= 0)
            {
                op.Message = "请先选择Excel文件";
                return op;
            }
            var stream = httpFiles[0].InputStream;
            var ext = httpFiles[0].FileName.Substring(httpFiles[0].FileName.LastIndexOf("."));
            if (!(ext.Equals(".xls", StringComparison.CurrentCultureIgnoreCase) ||
                ext.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase)))
            {
                op.Message = "请先选择Excel文件";
                return op;
            }
            var path="";
            var fullPath= Utility.Helpers.FileHelper.SaveAttachPath(ref path, "temps");
            httpFiles[0].SaveAs(fullPath + httpFiles[0].FileName);
            fieldCols = fieldCols ?? new Dictionary<string, char>();
            if (!fieldName.IsNullOrEmpty() && !columnName.IsNullOrEmpty())
            {
                var fields = fieldName.Split(',');
                var columns = columnName.Split(',');
                if (fields.Length != columns.Length)
                {
                    op.Message = "配置的字段和列数不一致!";
                    return op;
                }
                for (var i = 0; i < fields.Length; i++)
                {
                    if (columns[i].IsNullOrEmpty()) continue;
                    fieldCols[fields[i]] = Convert.ToChar(columns[i]);
                }
                //if (fieldCols.Values.Distinct().Count() != fieldCols.Values.Count())
                //{
                //    op.Message = "配置的列存在重复!";
                //    return op;
                //}
                obj.FieldJson = fieldCols.Select(o => new { o.Key, o.Value }).ToJson();
            }
            if (obj.Id == 0)
            {
                if (!BaseService<ImportSet>.IsExist(o => o.TableName == obj.TableName && o.CompanyId == obj.CompanyId))
                    op = BaseService<ImportSet>.Add(obj);
                else
                    op.Successed = true;
            }
            else
            {
                var res = BaseService<ImportSet>.FindById(obj.Id);
                obj.ToCopyProperty(res);
                op = BaseService<ImportSet>.Update(res);
            }
            if (!op.Successed) return op;

            dt = new ExportExcel().ToDataTable(stream, minRow: obj.MinRow, maxRow: obj.MaxRow.GetValueOrDefault());
            if (dt == null || dt.Rows.Count <= 0)
            {
                op.Message = "无数据，无法导入!";
                op.Successed = false;
                return op;
            }
            #region 去掉空格
            foreach (DataColumn dc in dt.Columns)
            {
                if(dc.DataType==typeof(string))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!(dr[dc] is DBNull))
                        {
                            dr[dc] = dr[dc].ToString().Trim();
                        }
                    }
                }
            }
            #endregion
            #region 允许配置在同一列
            var cols= fieldCols.GroupBy(o=>o.Value).Where(o=>o.Count()>1).ToList();//取重复列
            foreach (var item in cols)
            {
                System.Diagnostics.Debug.WriteLine(item.Key);//重复列value
                var idx = Convert.ToInt32(item.Key)-65;
                foreach (var subitem in item)
                {
                    System.Diagnostics.Debug.WriteLine(subitem.Key);//重复列key
                    var lastValue = Convert.ToChar(fieldCols.Values.OrderBy(o => o).LastOrDefault() + 1);
                    if (dt.Columns[idx].CloneTo(subitem.Key))
                        fieldCols[subitem.Key] = lastValue;
                }
            }
            #endregion

            op.Successed = true;
            return op;
        }
    }
}
