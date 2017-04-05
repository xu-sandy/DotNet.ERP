using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows;

namespace Pharos.Barcode.Retailing.Models
{
    public class LocalPrinter
    {
        private static PrintDocument fPrintDocument = new PrintDocument();
        //获取本机默认打印机名称
        public static String DefaultPrinter
        {
            get { return fPrintDocument.PrinterSettings.PrinterName; }
            set
            {
                if (!string.IsNullOrEmpty(value)) //判断是否有选中值
                {
                    if (WindowsApi.SetDefaultPrinter(value)) //设置默认打印机
                    {
                        MessageBox.Show(value + "设置为默认打印机成功！");
                    }
                    else
                    {
                        MessageBox.Show(value + "设置为默认打印机失败！");
                    }
                }
            }
        }
        public static List<String> GetLocalPrinters()
        {
            List<String> fPrinters = new List<String>();
            fPrinters.Add(DefaultPrinter); //默认打印机始终出现在列表的第一项
            foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (!fPrinters.Contains(fPrinterName))
                {
                    fPrinters.Add(fPrinterName);
                }
            }
            return fPrinters;
        }
    }
}
