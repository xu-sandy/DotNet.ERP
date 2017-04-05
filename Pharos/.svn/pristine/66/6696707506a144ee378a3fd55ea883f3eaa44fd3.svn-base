using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;

namespace Pharos.POS.Retailing.Models.Printer
{
    /// <summary>
    /// 打印常用操作
    /// </summary>
    public class PrintHelper
    {

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public static void Print(PrintData printData)
        {
            try
            {
                //现在只做POS打印机 其它打印类型暂不考虑
                bool isPreview = false;                                     //是否预览
                string printerType = "POS58";                              //打印机类型
                var printer = new PrintAllModel(printerType);
                printer.PrintDataList = printData.PrintDataList;
                printer.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("对不起,打印失败!未安装打印机!");
            }
        }


        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>

        public static void Print(string pringStr, string titleStr, bool isDayReport = false)
        {
            try
            {
                //现在只做POS打印机 其它打印类型暂不考虑
                bool isPreview = true;                                     //是否预览
                string printerType = "POS58";                              //打印机类型
                var printer = new PrintAllModel(printerType);
                printer.PrintStr = pringStr;
                printer.titleStr = titleStr;
                printer.isDayReport = isDayReport;
                printer.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("对不起,打印失败!未安装打印机!");
            }
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>


        public static void keyPress(object sender, KeyPressEventArgs e)
        {
            // if (e.KeyCode == Keys.Escape) { ((Form)sender).Close(); }
            if (e.KeyChar == (char)027)
            {
                ((Form)sender).Close();
                e.Handled = true;
            }
        }

    }
}

