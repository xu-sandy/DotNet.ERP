using Pharos.Barcode.Retailing.DeviceDrivers.ZMWINPrinter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;

namespace Pharos.Barcode.Retailing.Models.BarCodePriters.ZMIN
{
    public class ZMINPrinter : IPrinter
    {
        public void Print(string currentPrinter, Dtos.ProductDto product, BarCodePrintSettings format, uint printNum = 1, uint repeatNum = 1)
        {
            //var settings = format as ZMINPrinterSettings;
            //PrinterApi.OpenPort(currentPrinter);        //打开打印机端口，打印机驱动的名称一定要正确
            //PrinterApi.ZM_ClearBuffer();           //清空缓冲区
            //PrinterApi.ZM_SetPrintSpeed(settings.PrintSpeed);        //设置打印速度，4是4英寸/秒，6是6英寸/秒（X1最高是6，X1i最高是4）
            //PrinterApi.ZM_SetDarkness(settings.SetDarkness);         //设置打印黑度（也是打印温度，最高是20，一般蜡质碳带设置为10，混合基碳带设置为12，树脂基碳带设置为18）
            //PrinterApi.ZM_SetLabelHeight(settings.LabelHeight, settings.GapHeight); //设置标签的高度和标签之间的行间隙，里面的数值是像素值（200dpi是8点/毫米，300dpi是11.8点/毫米，如果计算出小数需取整）
            //PrinterApi.ZM_SetLabelWidth(settings.LabelWidth);      //设置标签的宽度
            //// 开始打印
            //try
            //{
            //    var formats = settings.Format.Split(new String[] { "@NEW@" }, StringSplitOptions.RemoveEmptyEntries);
            //    foreach (var item in formats)
            //    {
            //        var results = item.Split(new string[] { "&&&" }, StringSplitOptions.RemoveEmptyEntries);
            //        if (results.Count() > 1)
            //        {
            //            var configs = results[1].Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
            //            switch (results.FirstOrDefault())
            //            {
            //                case "Barcode":
            //                    DrawBarcode(product.Barcode, uint.Parse(configs[0]), uint.Parse(configs[1]), uint.Parse(configs[2]), uint.Parse(configs[3]), uint.Parse(configs[4]));
            //                    break;
            //                case "Text":
            //                    switch (configs[0])
            //                    {
            //                        case "@Barcode@":
            //                            DrawText(product.Barcode, int.Parse(configs[1]), int.Parse(configs[2]), int.Parse(configs[3]), int.Parse(configs[4]), configs[5]);
            //                            break;
            //                        case "@ProductTitle@":
            //                            DrawText(product.Title, int.Parse(configs[1]), int.Parse(configs[2]), int.Parse(configs[3]), int.Parse(configs[4]), configs[5]);
            //                            break;
            //                        case "@Price@":
            //                            DrawText(product.SysPrice.ToString("C", CultureInfo.CreateSpecificCulture("zh-CN")), int.Parse(configs[1]), int.Parse(configs[2]), int.Parse(configs[3]), int.Parse(configs[4]), configs[5]);
            //                            break;
            //                        default:
            //                            DrawText(configs[0], int.Parse(configs[1]), int.Parse(configs[2]), int.Parse(configs[3]), int.Parse(configs[4]), configs[5]);
            //                            break;
            //                    }
            //                    break;
            //            }
            //        }
            //    }
            //}
            //catch
            //{
            //    MessageBox.Show("模板解析失败！");
            //}


            //PrinterApi.ZM_PrinterApiel(printNum, repeatNum);//number：打印标签的数量，cpnumber:每张标签的复制份数
            ////关闭打印机端口
            //PrinterApi.ClosePort();


            if (product.IsWeigh)
            {
                PrinterApi.OpenPort("ZMIN X1");        //打开打印机端口，打印机驱动的名称一定要正确
                PrinterApi.ZM_ClearBuffer();           //清空缓冲区
                PrinterApi.ZM_SetPrintSpeed(4);        //设置打印速度，4是4英寸/秒，6是6英寸/秒（X1最高是6，X1i最高是4）
                PrinterApi.ZM_SetDarkness(10);         //设置打印黑度（也是打印温度，最高是20，一般蜡质碳带设置为10，混合基碳带设置为12，树脂基碳带设置为18）
                PrinterApi.ZM_SetLabelHeight(200, 16); //设置标签的高度和标签之间的行间隙，里面的数值是像素值（200dpi是8点/毫米，300dpi是11.8点/毫米，如果计算出小数需取整）
                PrinterApi.ZM_SetLabelWidth(800);      //设置标签的宽度

                PrintByType(3, product.ProductCode, product.Title, product.SysPrice, product.Unit);
                // 开始打印
                PrinterApi.ZM_PrintLabel(1, 1);//number：打印标签的数量，cpnumber:每张标签的复制份数

                //关闭打印机端口
                PrinterApi.ClosePort();
            }
            else if (product.Barcode.Length >= 13)
            {
                PrinterApi.OpenPort("ZMIN X1");        //打开打印机端口，打印机驱动的名称一定要正确
                PrinterApi.ZM_ClearBuffer();           //清空缓冲区
                PrinterApi.ZM_SetPrintSpeed(4);        //设置打印速度，4是4英寸/秒，6是6英寸/秒（X1最高是6，X1i最高是4）
                PrinterApi.ZM_SetDarkness(10);         //设置打印黑度（也是打印温度，最高是20，一般蜡质碳带设置为10，混合基碳带设置为12，树脂基碳带设置为18）
                PrinterApi.ZM_SetLabelHeight(200, 16); //设置标签的高度和标签之间的行间隙，里面的数值是像素值（200dpi是8点/毫米，300dpi是11.8点/毫米，如果计算出小数需取整）
                PrinterApi.ZM_SetLabelWidth(800);      //设置标签的宽度

                PrintByType(2, product.Barcode, product.Title, product.SysPrice, product.Unit);
                // 开始打印
                PrinterApi.ZM_PrintLabel(1, 1);//number：打印标签的数量，cpnumber:每张标签的复制份数

                //关闭打印机端口
                PrinterApi.ClosePort();
            }
            else if (product.Barcode.Length >= 10 && product.Barcode.Length < 13) 
            {
                PrinterApi.OpenPort("ZMIN X1");        //打开打印机端口，打印机驱动的名称一定要正确
                PrinterApi.ZM_ClearBuffer();           //清空缓冲区
                PrinterApi.ZM_SetPrintSpeed(2);        //设置打印速度，4是4英寸/秒，6是6英寸/秒（X1最高是6，X1i最高是4）
                PrinterApi.ZM_SetDarkness(8);         //设置打印黑度（也是打印温度，最高是20，一般蜡质碳带设置为10，混合基碳带设置为12，树脂基碳带设置为18）
                PrinterApi.ZM_SetLabelHeight(200, 16); //设置标签的高度和标签之间的行间隙，里面的数值是像素值（200dpi是8点/毫米，300dpi是11.8点/毫米，如果计算出小数需取整）
                PrinterApi.ZM_SetLabelWidth(800);      //设置标签的宽度

                PrintByType(1, product.Barcode, product.Title, product.SysPrice, product.Unit);
                // 开始打印
                PrinterApi.ZM_PrintLabel(1, 1);//number：打印标签的数量，cpnumber:每张标签的复制份数

                //关闭打印机端口
                PrinterApi.ClosePort();
            }
            //switch()
            //{

            //}
        }


        public void PrintByType(int type, string barCode, string productName, decimal price, string unit = "")
        {
            var name1 = string.Empty;
            var name2 = string.Empty;
            if (productName.Length <= 8)
            {
                name1 = productName;
            }
            else
            {
                name1 = productName.Substring(0, 8);
                name2 = productName.Substring(8, productName.Length - 8);
            }
            switch (type)
            {
                case 1:
                    PrinterApi.ZM_DrawTextTrueTypeW(640, 100, 25, 0, "Arial", 8, 600, false, false, false, "A1", name1);
                    PrinterApi.ZM_DrawTextTrueTypeW(615, 100, 25, 0, "Arial", 8, 600, false, false, false, "A2", name2);
                    if (price >= 1000)
                    {
                        PrinterApi.ZM_DrawTextTrueTypeW(580, 100, 25, 0, "Arial", 8, 600, false, false, false, "A3", "售价:" + price.ToString("f2"));
                    }
                    else
                    {
                        PrinterApi.ZM_DrawTextTrueTypeW(580, 100, 30, 0, "Arial", 8, 600, false, false, false, "A3", "售价:" + price.ToString("f2"));
                    }
                    if (barCode.Length > 12)
                    {
                        PrinterApi.ZM_DrawBarcode(470, 180, 3, "1", 1, 2, 50, 'B', barCode);
                    }
                    else if (barCode.Length > 8)
                    {
                        PrinterApi.ZM_DrawBarcode(470, 180, 3, "1A", 1, 2, 50, 'B', barCode);
                    }
                    else
                    {
                        PrinterApi.ZM_DrawBarcode(470, 180, 3, "1", 2, 2, 50, 'B', barCode);
                    }
                    break;
                case 2:
                    if (price >= 1000)
                    {
                        PrinterApi.ZM_DrawTextTrueTypeW(540, 10, 25, 0, "Arial", 7, 400, false, false, false, "A1", "售价:" + price.ToString("f2"));
                    }
                    else
                    {
                        PrinterApi.ZM_DrawTextTrueTypeW(540, 10, 30, 0, "Arial", 7, 400, false, false, false, "A1", "售价:" + price.ToString("f2"));
                    }
                    PrinterApi.ZM_DrawTextTrueTypeW(525, 40, 24, 0, "Arial", 7, 400, false, false, false, "A2", name2);
                    PrinterApi.ZM_DrawTextTrueTypeW(525, 65, 24, 0, "Arial", 7, 400, false, false, false, "A3", name1);
                    if (barCode.Length > 12)
                    {
                        PrinterApi.ZM_DrawBarcode(600, 175, 2, "1", 1, 2, 50, 'B', barCode);
                    }
                    else if (barCode.Length > 8)
                    {
                        PrinterApi.ZM_DrawBarcode(600, 175, 2, "1A", 1, 2, 50, 'B', barCode);
                    }
                    else
                    {
                        PrinterApi.ZM_DrawBarcode(600, 175, 2, "1", 2, 2, 50, 'B', barCode);
                    }

                    break;

                case 3:
                    if (barCode.Length > 12)
                    {
                        PrinterApi.ZM_DrawBarcode(520, 10, 0, "1", 1, 2, 40, 'B', barCode);
                    }
                    else if (barCode.Length > 8)
                    {
                        PrinterApi.ZM_DrawBarcode(520, 10, 0, "1A", 1, 2, 40, 'B', barCode);
                    }
                    else
                    {
                        PrinterApi.ZM_DrawBarcode(520, 10, 0, "1", 2, 2, 40, 'B', barCode);
                    }
                    PrinterApi.ZM_DrawTextTrueTypeW(600, 110, 25, 0, "Arial", 5, 400, false, false, false, "A1", name1);
                    PrinterApi.ZM_DrawTextTrueTypeW(600, 135, 25, 0, "Arial", 5, 400, false, false, false, "A2", name2);
                    if (price >= 1000)
                    {
                        PrinterApi.ZM_DrawTextTrueTypeW(590, 160, 25, 0, "Arial", 5, 400, false, false, false, "A3", price.ToString("f2") + unit);
                    }
                    else
                    {
                        PrinterApi.ZM_DrawTextTrueTypeW(590, 160, 30, 0, "Arial", 5, 400, false, false, false, "A3", price.ToString("f2") + unit);
                    }


                    break;


            }
        }
        private void DrawBarcode(string barcode, uint x, uint y, uint horizontal = 2, uint Vertical = 2, uint height = 50)
        {
            PrinterApi.ZM_DrawBarcode(x, y, 0, "1", horizontal, Vertical, height, 'B', barcode);
        }
        private void DrawText(string text, int x, int y, int fontSize, int fontSpin, string font = "Arial")
        {
            PrinterApi.ZM_DrawTextTrueTypeW(x, y, fontSize, 0, font, fontSpin, 400, false, false, false, "A2", text);
        }
    }
    public class ZMINPrinterSettings : BarCodePrintSettings
    {
        public ZMINPrinterSettings()
        {
            PrintSpeed = 6;
            SetDarkness = 18;
            LabelHeight = 200;
            LabelWidth = 800;
            GapHeight = 16;
        }
        public uint PrintSpeed { get; set; }

        public uint SetDarkness { get; set; }

        public uint LabelHeight { get; set; }

        public uint GapHeight { get; set; }

        public uint LabelWidth { get; set; }

        public string Format { get; set; }


        public string Key { get; set; }
    }

}
