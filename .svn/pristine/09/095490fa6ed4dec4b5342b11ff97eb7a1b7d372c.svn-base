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
            PrintLab.OpenPort("ZMIN X1i");//打开打印机端口
            PrintLab.ZM_ClearBuffer();           //清空缓冲区
            PrintLab.ZM_SetPrintSpeed(4);        //设置打印速度
            PrintLab.ZM_SetDarkness(10);         //设置打印黑度
            PrintLab.ZM_SetLabelHeight(600, 24); //设置标签的高度和定位间隙\黑线\穿孔的高度
            PrintLab.ZM_SetLabelWidth(800);      //设置标签的宽度

            for (int i = 1; i <= 1; i++)
            {

                // 画矩形
                PrintLab.ZM_DrawRectangle(50, 10, 3, 460, 340);
                // 画表格分割线
                PrintLab.ZM_DrawLineOr(60, 108, 402, 3);

                // 打印PCX图形
                PrintLab.ZM_PcxGraphicsDel("PCX");
                PrintLab.ZM_PcxGraphicsDownload("PCX", "pic.pcx");
                PrintLab.ZM_DrawPcxGraphics(80, 30, "PCX");

                // 打印一个128 Auto条码;
                PrintLab.ZM_DrawBarcode(80, 208, 0, "1", 2, 2, 50, 'B', "123456789");

                // 打印PDF417码
                PrintLab.ZM_DrawBar2D_Pdf417(80, 300, 400, 300, 0, 0, 3, 7, 10, 2, 0, 0, "123456789");//PDF417码

                // 打印QR码
                PrintLab.ZM_DrawBar2D_QR(360, 30, 180, 180, 0, 3, 2, 0, 0, "ZMIN Electronics Co., Ltd.");

                //打印内置字体点阵文字
                PrintLab.ZM_DrawText(80, 168, 0, 3, 1, 1, 'N', "Internal Font");

                // 打印WINDWOS系统TrueType Font文字;
                PrintLab.ZM_DrawTextTrueTypeW(80, 120, 40, 0, "Arial", 1, 400, false, false, false, "A1", "TrueType Font");

                // 打印WINDWOS系统TrueType Font文字（旋转90度);
                PrintLab.ZM_DrawTextTrueTypeW(420, 102, 22, 0, "Arial", 2, 400, false, false, false, "A2", "www.zmin.com.cn");


                // 开始打印
                PrintLab.ZM_PrintLabel(1, 1);

                //关闭打印机端口
                PrintLab.ClosePort();
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

