/*----------------------------------------------------------------
// 功能描述：导出Excel文件
 * 创 建 人：余雄文
// 创建时间：2015-08-23
//----------------------------------------------------------------*/

using System;
using System.Web;
using System.Linq;
using System.IO;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Security.Permissions;

using NPOI.HPSF;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Reflection;

namespace Pharos.Utility
{
    /// <summary>
    /// 导出Excel业务逻辑
    /// </summary>
    public class ExportExcelForCS
    {
        #region Excel To Collections
        /// <summary>
        /// 从Excel读取数据到列表中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <param name="ignoreHeaderAndColumn"></param>  
        /// <returns></returns>
        public static IList<T> ReadListFromStream<T>(T theEntity, object workBookObj, bool ignoreHeaderAndColumn) where T : new()
        {
            var workBook = workBookObj as IWorkbook;
            if (workBook == null) { throw new Exception("Excel表格工作簿为空"); }

            IList<T> list = new List<T>();
            for (int i = 0; i < workBook.NumberOfSheets; i++)
            {
                ISheet sheet = workBook.GetSheetAt(i);

                if (sheet.PhysicalNumberOfRows > 0)
                {
                    if (!ignoreHeaderAndColumn)
                    {
                        //检查列是否与ExcelAttribute定义的一致
                        ValidTableHeader<T>(sheet);
                    }

                    for (int j = ignoreHeaderAndColumn ? 0 : 2; j < sheet.PhysicalNumberOfRows; j++)
                    {
                        var row = sheet.GetRow(j);
                        T entity = new T();

                        var propertys = typeof(T).GetProperties();

                        foreach (var p in propertys)
                        {
                            var excel = Attribute.GetCustomAttribute(p, typeof(ExcelAttribute)) as ExcelAttribute;
                            if (excel != null)
                                SetProperty(row.GetCell(excel.ColumnIndex - 1, MissingCellPolicy.CREATE_NULL_AS_BLANK), p, entity);
                        }
                        list.Add(entity);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 检查表头与定义是否匹配
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstRow"></param>
        /// <returns></returns>
        private static void ValidTableHeader<T>(ISheet sheet) where T : new()
        {
            var firstRow = sheet.GetRow(1);

            var propertys = typeof(T).GetProperties();

            foreach (var p in propertys)
            {
                var excel = Attribute.GetCustomAttribute(p, typeof(ExcelAttribute)) as ExcelAttribute;

                if (excel != null)
                {
                    if (!firstRow.GetCell(excel.ColumnIndex - 1).StringCellValue.Trim().Equals(excel.ColumnName))
                    {
                        throw new Exception(string.Format("Excel表格第{0}列标题应为{1}", excel.ColumnIndex, excel.ColumnName));
                    }
                }
            }
        }
        public static object InitWorkbook(string fileName, Stream stream)
        {
            string extendsion = Path.GetExtension(fileName).TrimStart('.');

            IWorkbook workBook = null;
            switch (extendsion)
            {
                case "xls":
                    workBook = new HSSFWorkbook(stream);
                    break;
                case "xlsx":
                    workBook = new XSSFWorkbook(stream);
                    break;
            }
            return workBook;
        }
        public static string GetHeader(string fileName, Stream stream, object workBookObj)
        {
            var workBook = workBookObj as IWorkbook;
            if (workBook == null) { throw new Exception("Excel表格工作簿为空"); }
            ISheet sheet = workBook.GetSheetAt(0);
            var firstRow = sheet.GetRow(0);
            return firstRow.GetCell(0).StringCellValue.Trim();
        }
        #endregion Excel To Entity

        #region Collections To Excel
        /// <summary>
        /// 将集合写入Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="savePath"></param>
        /// <param name="dt"></param>
        /// <param name="maxRowInSheet"></param>
        public static void ToExcel<T>(T theEntity, string fileName, string savePath, IEnumerable<T> dt, int maxRowInSheet = 5000) where T : new()
        {
            if (dt == null || dt.Count() == 0)
            {
                return;
            }
            if (maxRowInSheet <= 0)
                maxRowInSheet = 5000;
            string extendsion = Path.GetExtension(fileName).TrimStart('.');

            IWorkbook workBook = null;
            switch (extendsion)
            {
                case "xls":
                    workBook = new HSSFWorkbook();
                    break;
                case "xlsx":
                    workBook = new XSSFWorkbook();
                    break;
                default:
                    throw new Exception("不支持的Excel格式！");
            }
            var type = theEntity.GetType();
            var titleAttr = Attribute.GetCustomAttribute(type, typeof(ExcelAttribute)) as ExcelAttribute;
            if (titleAttr == null)
                throw new Exception("传人实体未标记Sheet名称！");

            var properties = type.GetProperties();
            properties = properties.Where(o => Attribute.IsDefined(o, typeof(ExcelAttribute), true)).OrderBy(o => (Attribute.GetCustomAttribute(o, typeof(ExcelAttribute)) as ExcelAttribute).ColumnIndex).ToArray();


            ISheet sheet = null;
            var rowIndex = 0;
            for (var i = 0; i < dt.Count(); i++)
            {
                if (i % maxRowInSheet == 0)
                {
                    sheet = workBook.CreateSheet(string.Format("{0}({1})", titleAttr.Title, i / maxRowInSheet));
                    SetHeaderAndColumn(sheet, workBook, properties, titleAttr.Title);
                    rowIndex = 2;
                }
                IRow row = sheet.CreateRow(rowIndex);
                for (var j = 0; j < properties.Count(); j++)
                {
                    ICell cell = row.CreateCell(j);
                    SetCellValueForRow(cell, properties[j], dt.ElementAt(i), workBook);
                }
                rowIndex++;
            }

            string newFileName = Path.Combine(savePath, fileName);

            if (!Directory.Exists(savePath))
            {
                FileIOPermission ioPermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, savePath);
                ioPermission.Demand();
                Directory.CreateDirectory(savePath);
            }
            using (FileStream fs = new FileStream(newFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                workBook.Write(fs);
                fs.Dispose();
                fs.Close();
            }
        }
        public static void ToExcel<T>(string fileName, string savePath, IEnumerable<T> dt, int maxRowInSheet = 5000) where T : new()
        {
            if (dt == null || dt.Count() == 0)
            {
                return;
            }
            if (maxRowInSheet <= 0)
                maxRowInSheet = 5000;
            string extendsion = Path.GetExtension(fileName).TrimStart('.');

            IWorkbook workBook = null;
            switch (extendsion)
            {
                case "xls":
                    workBook = new HSSFWorkbook();
                    break;
                case "xlsx":
                    workBook = new XSSFWorkbook();
                    break;
                default:
                    throw new Exception("不支持的Excel格式！");
            }
            var type = dt.FirstOrDefault().GetType();
            var titleAttr = Attribute.GetCustomAttribute(type, typeof(ExcelAttribute)) as ExcelAttribute;
            if (titleAttr == null)
                throw new Exception("传人实体未标记Sheet名称！");

            var properties = type.GetProperties();
            properties = properties.Where(o => Attribute.IsDefined(o, typeof(ExcelAttribute), true)).OrderBy(o => (Attribute.GetCustomAttribute(o, typeof(ExcelAttribute)) as ExcelAttribute).ColumnIndex).ToArray();


            ISheet sheet = null;
            var rowIndex = 0;
            for (var i = 0; i < dt.Count(); i++)
            {
                if (i % maxRowInSheet == 0)
                {
                    sheet = workBook.CreateSheet(string.Format("{0}({1})", titleAttr.Title, i / maxRowInSheet));
                    SetHeaderAndColumn(sheet, workBook, properties, titleAttr.Title);
                    rowIndex = 2;
                }
                IRow row = sheet.CreateRow(rowIndex);
                for (var j = 0; j < properties.Count(); j++)
                {
                    ICell cell = row.CreateCell(j);
                    SetCellValueForRow(cell, properties[j], dt.ElementAt(i), workBook);
                }
                rowIndex++;
            }

            string newFileName = Path.Combine(savePath, fileName);

            if (!Directory.Exists(savePath))
            {
                FileIOPermission ioPermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, savePath);
                ioPermission.Demand();
                Directory.CreateDirectory(savePath);
            }
            using (FileStream fs = new FileStream(newFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                workBook.Write(fs);
                fs.Dispose();
                fs.Close();
            }
        }
        public static void SetHeaderAndColumn(ISheet sheet, IWorkbook workBook, PropertyInfo[] properties, string headerText)
        {
            properties = properties.Where(o => Attribute.IsDefined(o, typeof(ExcelAttribute), true)).ToArray();
            IFont titleFont = workBook.CreateFont();
            IFont headFont = workBook.CreateFont();
            IFont textFont = workBook.CreateFont();
            ICellStyle css = workBook.CreateCellStyle();



            //表头
            IRow header = sheet.CreateRow(0);
            header.HeightInPoints = 25;
            //样式
            ICellStyle cellStyle = workBook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;

            titleFont.FontHeightInPoints = 14;
            cellStyle.SetFont(titleFont);

            header.CreateCell(0).SetCellValue(headerText);
            header.GetCell(0).CellStyle = cellStyle;

            CellRangeAddress ra = new CellRangeAddress(0, 0, 0, properties.Count() - 1);
            sheet.AddMergedRegion(ra);

            //
            //列头
            header = sheet.CreateRow(1);
            header.HeightInPoints = 18;
            cellStyle = workBook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;

            headFont.FontHeightInPoints = 12;
            cellStyle.SetFont(headFont);

            for (int i = 0; i < properties.Count(); i++)
            {
                var attr = Attribute.GetCustomAttribute(properties[i], typeof(ExcelAttribute)) as ExcelAttribute;
                header.CreateCell(i).SetCellValue(attr.ColumnName);
                header.GetCell(i).CellStyle = cellStyle;
                var colWidth = Encoding.GetEncoding(936).GetBytes(attr.ColumnName).Length;
                colWidth = (colWidth < 5) ? 5 : colWidth + 2;
                sheet.SetColumnWidth(i, (int)Math.Ceiling((double)((colWidth + 1) * 256)));
            }

        }
        public static void SetCellValueForRow<T>(ICell cell, PropertyInfo propertyInfo, T entity, IWorkbook workBook) where T : new()
        {
            try
            {
                var val = propertyInfo.GetValue(entity, null);
                switch (propertyInfo.PropertyType.ToString())
                {
                    case "System.String":

                        if (val != null)
                        {
                            cell.SetCellValue(val.ToString());
                        }
                        else
                        {
                            cell.SetCellValue("");
                        }
                        break;
                    case "System.DateTime":
                        try
                        {
                            ICellStyle cellStyle = workBook.CreateCellStyle();
                            IDataFormat format = workBook.CreateDataFormat();
                            cellStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");
                            cell.CellStyle = cellStyle;
                            var dt = Convert.ToDateTime(val);
                            cell.SetCellValue(dt);
                        }
                        catch
                        {
                            cell.SetCellValue("");
                        }

                        break;
                    case "System.Boolean":
                        try
                        {
                            var bl = Convert.ToInt32(val);
                            cell.SetCellValue(bl);
                        }
                        catch
                        {
                            cell.SetCellValue("");
                        }
                        break;
                    case "System.Byte":
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Decimal":
                    case "System.Double":
                        double dbl = 0;
                        try
                        {
                            dbl = Convert.ToDouble(val);
                            cell.SetCellValue(dbl);
                        }
                        catch
                        {
                            cell.SetCellValue(dbl);
                        }
                        break;
                    case "System.DBNull":
                    default:
                        cell.SetCellValue("");
                        break;

                }
            }
            catch
            {

            }
        }


        public static void SetProperty<T>(ICell cell, PropertyInfo propertyInfo, T entity) where T : new()
        {
            var cellValue = Convert.ToString(cell);
            var FieldAttr = propertyInfo.GetCustomAttributes(typeof(ExcelFieldAttribute), true);
            foreach (var item in FieldAttr)
            {
                var attr = item as ExcelFieldAttribute;
                var result = attr.VerifyFiled(cellValue);
                if (!result.IsSuccess)
                {
                    throw new Exception(result.Message + "位置：" + cell.RowIndex + "行" + cell.ColumnIndex + "列");
                }
            }
            if (string.IsNullOrEmpty(cellValue))
            {
                return;
            }
            try
            {
                switch (propertyInfo.PropertyType.ToString())
                {
                    case "System.String":
                        propertyInfo.SetValue(entity, cellValue, null);
                        break;
                    case "System.DateTime":
                        var dt = Convert.ToDateTime(cellValue);
                        propertyInfo.SetValue(entity, dt, null);
                        break;
                    case "System.Boolean":
                        var bl = Convert.ToBoolean(Convert.ToInt32(cellValue));
                        propertyInfo.SetValue(entity, bl, null);
                        break;
                    case "System.Byte":
                        propertyInfo.SetValue(entity, Convert.ToByte(cellValue), null);
                        break;
                    case "System.Int16":
                        propertyInfo.SetValue(entity, Convert.ToInt16(cellValue), null);
                        break;
                    case "System.Int32":
                        propertyInfo.SetValue(entity, Convert.ToInt32(cellValue), null);
                        break;
                    case "System.Int64":
                        propertyInfo.SetValue(entity, Convert.ToInt64(cellValue), null);
                        break;
                    case "System.Decimal":
                        propertyInfo.SetValue(entity, Convert.ToDecimal(cellValue), null);
                        break;
                    case "System.Double":
                        double dbl = 0;
                        dbl = Convert.ToDouble(cellValue);
                        propertyInfo.SetValue(entity, dbl, null);
                        break;
                }
            }
            catch (Exception)
            {
                throw new Exception("类型转换失败！" + "位置：" + cell.RowIndex + "行" + cell.ColumnIndex + "列");
            }
        }

        #endregion Collections To Excel

    }


    public interface IWorkbookForClient : IWorkbook
    {

    }

}
