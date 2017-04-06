using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using System.Linq;

namespace Pharos.POS.Retailing.Models.Printer
{
    public class PrintAllModel
    {

        /// <summary>
        /// 打印文档
        /// </summary>
        public PrintDocument printDoc;
        protected bool m_bIsPreview;
        protected bool isPrint = true;
        protected int iNowIndex = 0;
        protected bool m_bIsMorePage = false;
        protected int ctrlSpace = 0;


        //打印有效区域的宽度
        protected int width;
        protected int height;
        protected int curX = 1;
        protected int curY;

        protected int PageWidth = 58; //打印纸的宽度
        protected int PageHeight; //打印纸的高度
        protected int LeftMargin = 5; //有效打印区距离打印纸的左边大小
        protected int TopMargin;//有效打印区距离打印纸的上面大小
        protected int RightMargin = 5;//有效打印区距离打印纸的右边大小
        protected int BottomMargin;//有效打印区距离打印纸的下边大小

        protected Font fontBody = new Font("宋体", 11, FontStyle.Regular, GraphicsUnit.World);
        protected Brush defaultBrush = Brushes.Black;
        protected Pen defaultPen = new Pen(Brushes.Black, 1);
        protected int lineSpace = 1;



        /// <summary>
        /// wyh 2013-3-2 
        /// 累加高度
        /// </summary>
        private int _cumulativeHeight = 0;

        /// <summary>
        /// 打印相关数据
        /// </summary>
        public DataTable PrintDataList = null;

        /// <summary>
        /// 打印相关数据
        /// </summary>
        public string PrintStr = null;


        public ArrayList tailItems = new ArrayList();


        public string tailNoteString = "";


        int pageWidth = 0;
        int margLeft = 0;
        Font tmpFont = new Font("宋体", 12, FontStyle.Regular, GraphicsUnit.World);
        Font dayReportFont = new Font("宋体", 11, FontStyle.Regular, GraphicsUnit.World);
        Graphics g = null;

        RectangleF rect;
        string _Txt = "";
        string _TestStr = "";
        string _Bind = "";
        StringAlignment strAlign = StringAlignment.Near;

        int fontHeight = 0;
        int tableRowcount = 0;

        Font CFont = new Font("楷体_GB2312", 12, FontStyle.Regular, GraphicsUnit.World);

        int CFontSize = 12;
        Font RFont = new Font("楷体_GB2312", 12, FontStyle.Regular, GraphicsUnit.World);

        int RFontSize = 12;


        string POSTYPE;
        string BILLTYPE;
        string modelPath = "";
        /// <summary>
        /// 打印份数
        /// </summary>
        public short iPrintCopies = 1;

        //liu 2011-10-20 保存fontHeight的值,防止打印文字重叠。
        int fontHeightSave = 0;

        ArrayList tables = new ArrayList();

        public PrintAllModel(string postype)
        {

            ConstructPrintModel(postype);

        }

        public void ConstructPrintModel(string postype)
        {

            this.POSTYPE = postype;

            this.printDoc = new PrintDocument();
            this.printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
            DataTable Bodydt;

        }




        string typede = "";
        bool istrue = true;
        public string titleStr = string.Empty;
        public bool isDayReport = false;
        protected void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            g = e.Graphics;
            DrawOneLine(g, titleStr, true);
            DrawOneLine(g, PrintStr, false);
            return;


        }



        private bool isPrinting = false;

        public void Print()
        {
            isPrinting = true;
            printDoc.Print();
            isPrinting = false;
        }

        int h = 0;
        //打一行数据
        protected void DrawOneLine(Graphics g, string strLine, bool isTitle = false)
        {
            if (isDayReport)
            {
                int fontHeight = Convert.ToInt32(g.MeasureString(strLine, dayReportFont).Height);
                int fontHeighto = fontHeight;
                Rectangle tmpRect = new Rectangle(curX, curY, width, fontHeighto + lineSpace);
                RectangleF textRect = new RectangleF(curX, curY, tmpRect.Width, tmpRect.Height);
                LeftText(g, strLine, dayReportFont, defaultBrush, textRect);
                curY += fontHeight + lineSpace;
            }
            else
            {
                if (isTitle)
                {//标题

                    int fontHeight = Convert.ToInt32(g.MeasureString(strLine, tmpFont).Height);
                    int fontHeighto = fontHeight;
                    Rectangle tmpRect = new Rectangle(curX, curY, width, fontHeighto);
                    RectangleF textRect = new RectangleF(curX, curY, tmpRect.Width, tmpRect.Height);
                    LeftText(g, strLine, tmpFont, defaultBrush, textRect);
                    curY += fontHeight;
                }
                else
                {
                    int fontHeight = Convert.ToInt32(g.MeasureString(strLine, fontBody).Height);
                    int fontHeighto = fontHeight;
                    Rectangle tmpRect = new Rectangle(curX, curY, width, fontHeighto + lineSpace);
                    RectangleF textRect = new RectangleF(curX, curY, tmpRect.Width, tmpRect.Height);
                    LeftText(g, strLine, fontBody, defaultBrush, textRect);
                    curY += fontHeight + lineSpace;
                }
            }
        }

        protected void LeftText(Graphics g, string t, Font f, Brush b, RectangleF rect)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            RectangleF rectone = rect;

            g.DrawString(t, f, b, rect, sf);
        }






        public PrintEventHandler printDoc_BeginPrint { get; set; }



    }
}