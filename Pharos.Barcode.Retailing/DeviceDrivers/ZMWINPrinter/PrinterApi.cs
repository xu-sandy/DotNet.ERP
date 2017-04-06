using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Pharos.Barcode.Retailing.DeviceDrivers.ZMWINPrinter
{
    public static class PrinterApi
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
}
