using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PrinterDEMO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintLab.OpenPort("ZMIN X1");        //打开打印机端口，打印机驱动的名称一定要正确
            PrintLab.ZM_ClearBuffer();           //清空缓冲区
            PrintLab.ZM_SetPrintSpeed(4);        //设置打印速度，4是4英寸/秒，6是6英寸/秒（X1最高是6，X1i最高是4）
            PrintLab.ZM_SetDarkness(10);         //设置打印黑度（也是打印温度，最高是20，一般蜡质碳带设置为10，混合基碳带设置为12，树脂基碳带设置为18）
            PrintLab.ZM_SetLabelHeight(200, 16); //设置标签的高度和标签之间的行间隙，里面的数值是像素值（200dpi是8点/毫米，300dpi是11.8点/毫米，如果计算出小数需取整）
            PrintLab.ZM_SetLabelWidth(800);      //设置标签的宽度

            PrintByType(1);
            // 开始打印
            PrintLab.ZM_PrintLabel(1, 1);//number：打印标签的数量，cpnumber:每张标签的复制份数

            //关闭打印机端口
            PrintLab.ClosePort();


            PrintLab.OpenPort("ZMIN X1");        //打开打印机端口，打印机驱动的名称一定要正确
            PrintLab.ZM_ClearBuffer();           //清空缓冲区
            PrintLab.ZM_SetPrintSpeed(4);        //设置打印速度，4是4英寸/秒，6是6英寸/秒（X1最高是6，X1i最高是4）
            PrintLab.ZM_SetDarkness(10);         //设置打印黑度（也是打印温度，最高是20，一般蜡质碳带设置为10，混合基碳带设置为12，树脂基碳带设置为18）
            PrintLab.ZM_SetLabelHeight(200, 16); //设置标签的高度和标签之间的行间隙，里面的数值是像素值（200dpi是8点/毫米，300dpi是11.8点/毫米，如果计算出小数需取整）
            PrintLab.ZM_SetLabelWidth(800);      //设置标签的宽度

            PrintByType(2);
            // 开始打印
            PrintLab.ZM_PrintLabel(1, 1);//number：打印标签的数量，cpnumber:每张标签的复制份数

            //关闭打印机端口
            PrintLab.ClosePort();
        }


        public void PrintByType(int type)
        {
            switch (type)
            {
                case 1:
                    PrintLab.ZM_DrawBarcode(150, 10, 0, "1", 2, 2, 50, 'B', "0123456789123");
                    PrintLab.ZM_DrawTextTrueTypeW(170, 120, 40, 0, "Arial", 1, 400, false, false, false, "A1", "￥15.00");
                    PrintLab.ZM_DrawTextTrueTypeW(500, 70, 30, 0, "Arial", 2, 400, false, false, false, "A2", "Tab1");
                    PrintLab.ZM_DrawTextTrueTypeW(620, 70, 30, 0, "Arial", 2, 400, false, false, false, "A3", "Tab2");
                    break;
                case 2:
                    PrintLab.ZM_DrawBarcode(150, 10, 0, "1", 2, 2, 50, 'B', "0123456789123");
                    PrintLab.ZM_DrawBarcode(150, 115, 0, "1", 2, 2, 50, 'B', "6912345678901");
                    PrintLab.ZM_DrawTextTrueTypeW(500, 70, 30, 0, "Arial", 2, 400, false, false, false, "A2", "Tab1");
                    PrintLab.ZM_DrawTextTrueTypeW(620, 70, 30, 0, "Arial", 2, 400, false, false, false, "A3", "Tab2");
                    break;
            }
        }
    }

}



public class PrintLab
{
    [DllImport("ZMWIN.dll")]
    public static extern int OpenPort(string printname);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_SetPrintSpeed(uint px);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_SetDarkness(uint id);
    [DllImport("ZMWIN.dll")]
    public static extern int ClosePort();
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_PrintLabel(uint number, uint cpnumber);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_DrawTextTrueTypeW
                                        (int x, int y, int FHeight,
                                        int FWidth, string FType,
                                        int Fspin, int FWeight,
                                        bool FItalic, bool FUnline,
                                        bool FStrikeOut,
                                        string id_name,
                                        string data);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_DrawBarcode(uint px,
                                    uint py,
                                    uint pdirec,
                                    string pCode,
                                    uint pHorizontal,
                                    uint pVertical,
                                    uint pbright,
                                    char ptext,
                                    string pstr);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_SetLabelHeight(uint lheight, uint gapH);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_SetLabelWidth(uint lwidth);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_ClearBuffer();
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_DrawRectangle(uint px, uint py, uint thickness, uint pEx, uint pEy);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_DrawLineOr(uint px, uint py, uint pLength, uint pH);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_DrawBar2D_QR(uint x, uint y, uint w, uint v, uint o, uint r, uint m, uint g, uint s, string pstr);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_DrawBar2D_Pdf417(uint x, uint y, uint w, uint v, uint s, uint c, uint px, uint py, uint r, uint l, uint t, uint o, string pstr);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_PcxGraphicsDel(string pid);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_PcxGraphicsDownload(string pcxname, string pcxpath);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_DrawPcxGraphics(uint px, uint py, string gname);
    [DllImport("ZMWIN.dll")]
    public static extern int ZM_DrawText(uint px, uint py, uint pdirec, uint pFont, uint pHorizontal, uint pVertical, char ptext, string pstr);


}

