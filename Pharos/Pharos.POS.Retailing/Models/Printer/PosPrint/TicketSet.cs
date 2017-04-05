﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Pharos.POS.Retailing.Models.Printer
{
    public class TicketSet
    {
        public struct keyAndValue
        {
            public String keyStr;
            public String valueStr;
        }
        private List<keyAndValue> keyAndValueListTop = new List<keyAndValue>();
        /// <summary>
        /// 小票头部信息
        /// </summary>
        public List<keyAndValue> KeyAndValueListTop
        {
            get { return keyAndValueListTop; }
        }
        /// <summary>
        /// 增加小票头部键值对
        /// </summary>
        /// <param name="keyStr">键</param>
        /// <param name="valueStr">值</param>
        public void AddKeyAndValueTop(String keyStr, String valueStr)
        {
            keyAndValue keyandvale = new keyAndValue();
            keyandvale.keyStr = keyStr;
            keyandvale.valueStr = valueStr;
            this.keyAndValueListTop.Add(keyandvale);
        }
        private List<keyAndValue> keyAndValueListMid = new List<keyAndValue>();
        /// <summary>
        /// 小票中间费用信息
        /// </summary>
        public List<keyAndValue> KeyAndValueListMid
        {
            get { return keyAndValueListMid; }
        }
        private List<keyAndValue> keyAndValueListForCard = new List<keyAndValue>();
        /// <summary>
        /// 会员卡信息
        /// </summary>
        public List<keyAndValue> KeyAndValueListForCard
        {
            get { return keyAndValueListForCard; }
        }
        /// <summary>
        /// 增加小票中间费用键值对
        /// </summary>
        /// <param name="keyStr">键</param>
        /// <param name="valueStr">值</param>
        public void AddKeyAndValueMid(String keyStr, String valueStr)
        {
            keyAndValue keyandvale = new keyAndValue();
            keyandvale.keyStr = keyStr;
            keyandvale.valueStr = valueStr;
            this.keyAndValueListMid.Add(keyandvale);
        }
        /// <summary>
        /// 增加小票中间会员卡信息键值对
        /// </summary>
        /// <param name="keyStr">键</param>
        /// <param name="valueStr">值</param>
        public void AddKeyAndValueListForCard(String keyStr, String valueStr)
        {
            keyAndValue keyandvale = new keyAndValue();
            keyandvale.keyStr = keyStr;
            keyandvale.valueStr = valueStr;
            this.keyAndValueListForCard.Add(keyandvale);
        }
        private List<keyAndValue> keyAndValueListFoot = new List<keyAndValue>();
        /// <summary>
        /// 小票底部信息
        /// </summary>
        public List<keyAndValue> KeyAndValueListFoot
        {
            get { return keyAndValueListFoot; }
        }
        /// <summary>
        /// 增加小票底部键值对
        /// </summary>
        /// <param name="keyStr">键</param>
        /// <param name="valueStr">值</param>
        public void AddKeyAndValueFoot(String keyStr, String valueStr)
        {
            keyAndValue keyandvale = new keyAndValue();
            keyandvale.keyStr = keyStr;
            keyandvale.valueStr = valueStr;
            this.keyAndValueListFoot.Add(keyandvale);
        }
        private String ticketSignature;
        /// <summary>
        /// 小票顶部签名
        /// </summary>
        public String TicketSignature
        {
            get { return ticketSignature; }
            set { ticketSignature = value; }
        }
        private String ticketTitle;
        /// <summary>
        /// 小票的标题
        /// </summary>
        public String TicketTitle
        {
            get { return ticketTitle; }
            set { ticketTitle = value; }
        }
        private String ticketFooter;
        /// <summary>
        /// 小票底部签名
        /// </summary>
        public String TicketFooter
        {
            get { return ticketFooter; }
            set { ticketFooter = value; }
        }
        private DataTable dtGoodsList;
        /// <summary>
        /// 商品列表信息
        /// 传入DataTable的格式
        /// 商品编码    商品名称    数量    单价    金额
        /// </summary>
        public DataTable DtGoodsList
        {
            get { return dtGoodsList; }
            set { dtGoodsList = value; }
        }
        private int ticketWidth;
        /// <summary>
        /// 小票宽度,按字符数计算
        /// </summary>
        public int TicketWidth
        {
            get { return ticketWidth; }
            set { ticketWidth = value; }
        }
        private Decimal colper1;
        /// <summary>
        /// 商品列表中第一个标题所占小票总宽度的百分比
        /// </summary>
        public Decimal Colper1
        {
            get { return colper1; }
            set { colper1 = value; }
        }
        private Decimal colper2;
        /// <summary>
        /// 商品列表中第二个标题所占小票总宽度的百分比
        /// </summary>
        public Decimal Colper2
        {
            get { return colper2; }
            set { colper2 = value; }
        }
        private Decimal colper3;
        /// <summary>
        /// 商品列表中第三个标题所占小票总宽度的百分比
        /// </summary>
        public Decimal Colper3
        {
            get { return colper3; }
            set { colper3 = value; }
        }
        private Decimal colper4;
        /// <summary>
        /// 商品列表中第四个标题所占小票总宽度的百分比
        /// </summary>
        public Decimal Colper4
        {
            get { return colper4; }
            set { colper4 = value; }
        }

        private Decimal keyColper;
        /// <summary>
        /// 键所占总宽度百分比
        /// </summary>
        public Decimal KeyColper
        {
            get { return keyColper; }
            set { keyColper = value; }
        }

        private Decimal valueColper;
        /// <summary>
        /// 值所占总宽度百分比
        /// </summary>
        public Decimal ValueColper
        {
            get { return valueColper; }
            set { valueColper = value; }
        }

        private Decimal keyTopColper;
        /// <summary>
        /// 头部键所占总宽度百分比
        /// </summary>
        public Decimal KeyTopColper
        {
            get { return keyTopColper; }
            set { keyTopColper = value; }
        }

        private Decimal valueTopColper;
        /// <summary>
        /// 头部值所占总宽度百分比
        /// </summary>
        public Decimal ValueTopColper
        {
            get { return valueTopColper; }
            set { valueTopColper = value; }
        }


        private Char signWeight;
        /// <summary>
        /// 重要分隔符的样式
        /// </summary>
        public Char SignWeight
        {
            get { return signWeight; }
            set { signWeight = value; }
        }
        private Char signLight;
        /// <summary>
        /// 一般分隔符的样式
        /// </summary>
        public Char SignLight
        {
            get { return signLight; }
            set { signLight = value; }
        }

        private Decimal midCol1KeyColper;
        /// <summary>
        /// 中部第一列键所占总宽度百分比
        /// </summary>
        public Decimal MidCol1KeyColper
        {
            get { return midCol1KeyColper; }
            set { midCol1KeyColper = value; }
        }

        private Decimal midCol1valueColper;
        /// <summary>
        /// 中部第一列值所占总宽度百分比
        /// </summary>
        public Decimal MidCol1ValueColper
        {
            get { return midCol1valueColper; }
            set { midCol1valueColper = value; }
        }

        private DayReportModel dayReportModel;
        public DayReportModel DayReportModel
        {
            get { return dayReportModel; }
            set { dayReportModel = value; }
        }

        /// <summary>
        /// 商品列表设置
        /// </summary>
        /// <param name="ticket">TicketSet对象</param>
        /// 商品列表格式
        /// 商品名称    数量    单价    金额
        /// 李宁牌运动上衣
        /// 00009        1      290     290
        /// <returns>带格式的商品列表</returns>
        private String ItemsList(bool isRecharge)
        {
            StringBuilder result = new StringBuilder();

            if (this.dtGoodsList != null && this.dtGoodsList.Columns.Count > 0 && this.dtGoodsList.Rows.Count > 0)
            {
                result.Append(CreateLine(this.TicketWidth, this.SignWeight));
                result.Append(ArrangeArgPosition(this.dtGoodsList.Columns[1].Caption, this.TicketWidth, this.Colper1));
                //result.Append(ArrangeArgPosition(this.dtGoodsList.Columns[2].Caption, this.TicketWidth, this.Colper2));
                result.Append(ArrangeArgPosition(this.dtGoodsList.Columns[2].Caption, this.TicketWidth, this.Colper3));
                result.Append(ArrangeArgPosition(this.dtGoodsList.Columns[3].Caption, this.TicketWidth, this.Colper4));
                result.Append("\r\n");
                //result.Append(CreateLine(this.ticketWidth, this.SignLight));
                for (int i = 0; i < this.dtGoodsList.Rows.Count; i++)
                {
                    if (isRecharge)
                    {//充值的网格
                        result.Append(ArrangeArgPosition(this.dtGoodsList.Rows[i][1].ToString(), this.TicketWidth, this.Colper1));
                        result.Append(ArrangeArgPosition(this.dtGoodsList.Rows[i][2].ToString(), this.TicketWidth, this.Colper3));
                        result.Append(ArrangeArgPosition(this.dtGoodsList.Rows[i][3].ToString(), this.TicketWidth, this.Colper4));
                    }
                    else
                    {
                        //商品名称
                        result.Append(SetArgPosition(this.dtGoodsList.Rows[i][1].ToString(), this.TicketWidth, false));

                        //商品编码
                        result.Append(ArrangeArgPosition(this.dtGoodsList.Rows[i][0].ToString(), this.TicketWidth, this.Colper1));
                        //数量
                        //result.Append(ArrangeArgPosition(this.dtGoodsList.Rows[i][2].ToString(), this.TicketWidth, this.Colper2));
                        //单价
                        result.Append(ArrangeArgPosition(this.dtGoodsList.Rows[i][2].ToString(), this.TicketWidth, this.Colper3));
                        //金额
                        result.Append(ArrangeArgPosition(this.dtGoodsList.Rows[i][3].ToString(), this.TicketWidth, this.Colper4));
                    }
                    result.Append("\r\n");
                }
                result.Append(CreateLine(this.ticketWidth, this.SignLight));

            }
            return result.ToString();
        }
        /// <summary>
        /// 排列商品列表的表头信息
        /// </summary>
        /// <param name="arg">表头标题</param>
        /// <param name="charNum">标题所占总字符数，一般按照小票总宽度的百分比来设置</param>
        /// <param name="isJustifyLeft">是否左对齐</param>
        /// <returns>带有格式的标题</returns>
        private String ArrangeArgPosition(String arg, int ticketwidth, Decimal colPer, bool isAlignRight = false)
        {
            StringBuilder result = new StringBuilder();
            if (!isAlignRight)
            {
                result.Append(arg);
            }
            else
            {
                arg = arg + " ";
            }
            int charNum = Convert.ToInt32(ticketWidth * colPer);
            if (0 != charNum)
            {
                int argcount = System.Text.Encoding.Default.GetByteCount(arg);
                for (int i = 0; i < charNum - argcount; i++)
                {
                    result.Append(" ");
                }
                if (isAlignRight)
                {
                    result.Append(arg);
                }
            }
            else
            {
                //隐藏某一列
                result.Remove(0, result.Length);
            }
            return result.ToString();
        }


        /// <summary>
        /// 设置小票头部信息，可以自动区分汉字还是英文，格式只限2行
        /// </summary>
        /// <param name="arg">小票头部内容</param>
        /// <param name="ticketwidth">小票宽度，按照字符个数计算</param>
        /// <param name="isMiddle">是否中间对齐</param>
        /// <returns>带格式的小票头部信息</returns>
        private String SetArgPosition(String arg, int ticketwidth, bool isMiddle)
        {
            StringBuilder result = new StringBuilder();
            int argnum = System.Text.Encoding.Default.GetByteCount(arg);
            if (argnum <= ticketwidth)
            {
                if (isMiddle)
                {
                    for (int i = 0; i < (ticketwidth - argnum) / 2; i++)
                    {
                        result.Append(" ");
                    }
                }
                result.Append(arg);
                result.Append("\r\n");
            }
            else
            {
                for (int i = 0; i <= ticketwidth / 2; i++)
                {
                    int temp = ticketwidth / 2 + i;
                    if (ticketwidth == System.Text.Encoding.Default.GetByteCount(arg.Substring(0, temp)) || ticketwidth == System.Text.Encoding.Default.GetByteCount(arg.Substring(0, temp)) - 1)
                    {
                        result.Append(arg.Substring(0, temp));
                        result.Append("\r\n");
                        result.Append(arg.Substring(temp, arg.Length - (temp)));
                        result.Append("\r\n");
                        break;
                    }
                }
            }
            return result.ToString();

        }
        /// <summary>
        /// 设置小票各部分的分隔线
        /// </summary>
        /// <param name="ticketwidth">小票的宽度，按照字符个数计算</param>
        /// <param name="signChar">分隔线的样式</param>
        /// <returns>小票的分隔线</returns>
        private String CreateLine(int ticketwidth, Char signChar)
        {

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < ticketwidth; i++)
            {
                result.Append(signChar);
            }
            result.Append("\r\n");
            return result.ToString();
        }
        /// <summary>
        /// 生成小票
        /// </summary>
        /// <param name="ticket">TicketSet对象</param>
        /// <returns>最终小票结果</returns>
        public String Ticket(bool isRecharge = false)
        {
            StringBuilder ticketStr = new StringBuilder();
            //小票头部
            //ticketStr.Append(SetArgPosition(this.TicketSignature, this.TicketWidth, true));
            //ticketStr.Append(SetArgPosition(this.TicketTitle, this.TicketWidth, true));
            ticketStr.Append(CreateLine(this.TicketWidth, this.signWeight));
            //小票上部内容
            for (int i = 0; i < this.KeyAndValueListTop.Count; i++)
            {
                if (!this.KeyAndValueListTop[i].keyStr.Contains("时间：") && !this.KeyAndValueListTop[i].keyStr.Contains("流水号：") && !this.KeyAndValueListTop[i].keyStr.Contains("充值卡号："))
                {
                    ticketStr.Append(ArrangeArgPosition(this.KeyAndValueListTop[i].keyStr, this.TicketWidth, this.KeyTopColper) + ArrangeArgPosition(this.KeyAndValueListTop[i].valueStr, this.TicketWidth, this.ValueTopColper));
                    if ((i + 1) % 2 == 0)
                    {
                        ticketStr.Append("\n");
                    }
                    else if (i == this.KeyAndValueListTop.Count - 1 && (i + 1) % 2 != 0)
                    {
                        ticketStr.Append("\n");
                    }
                }
                else
                {
                    ticketStr.Append(ArrangeArgPosition(this.KeyAndValueListTop[i].keyStr, this.TicketWidth, this.KeyTopColper - 0.03M) + this.KeyAndValueListTop[i].valueStr);
                    ticketStr.Append("\n");
                }
            }
            //商品列表
            ticketStr.Append(ItemsList(isRecharge));
            //小票中部内容
            for (int i = 0; i < this.KeyAndValueListMid.Count; i++)
            {
                ticketStr.Append(ArrangeArgPosition(this.KeyAndValueListMid[i].keyStr, this.TicketWidth, this.MidCol1KeyColper) + ArrangeArgPosition(this.KeyAndValueListMid[i].valueStr, this.TicketWidth, this.MidCol1ValueColper));

                if ((i + 1) % 2 == 0)
                {
                    ticketStr.Append("\n");
                }
                else if (i == this.KeyAndValueListMid.Count - 1 && (i + 1) % 2 != 0)
                {
                    ticketStr.Append("\n");
                }

            }
            //非充值小票的话 会员卡支付的信息
            if (!isRecharge)
            {
                if (this.KeyAndValueListForCard != null && this.KeyAndValueListForCard.Count > 0)
                {
                    ticketStr.Append(CreateLine(this.TicketWidth, this.signWeight));

                    for (int i = 0; i < this.KeyAndValueListForCard.Count; i++)
                    {
                        ticketStr.Append(ArrangeArgPosition(this.KeyAndValueListForCard[i].keyStr, this.TicketWidth, this.MidCol1KeyColper) + ArrangeArgPosition(this.KeyAndValueListForCard[i].valueStr, this.TicketWidth, this.MidCol1ValueColper));
                        ticketStr.Append("\n");
                    }
                }
            }


            ticketStr.Append(CreateLine(this.TicketWidth, this.signWeight));

            //小票下部内容
            for (int i = 0; i < this.KeyAndValueListFoot.Count; i++)
            {
                ticketStr.Append(SetArgPosition(this.KeyAndValueListFoot[i].keyStr + this.KeyAndValueListFoot[i].valueStr, this.TicketWidth, true));
            }
            //小票底部
            ticketStr.Append(SetArgPosition(this.TicketFooter, this.TicketWidth, true));
            //ticketStr.Append(SetArgPosition(this.TicketFooter, this.TicketWidth, true));
            //ticketStr.Append(SetArgPosition(this.TicketFooter, this.TicketWidth, true));

            //ticketStr.Append(CreateLine(this.TicketWidth, this.signWeight));
            ticketStr.Append("\r\n");
            ticketStr.Append(CreateLine(this.TicketWidth, this.signWeight));
            return ticketStr.ToString();

        }


        public string GetTicketTitle()
        {
            StringBuilder ticketStr = new StringBuilder();
            //小票头部
            ticketStr.Append(SetArgPosition(this.TicketTitle, this.TicketWidth - 10, true));
            string title = ticketStr.ToString().Replace("\n", string.Empty).Replace("\r", string.Empty);
            return title;
        }

        /// <summary>
        /// 生成日结单
        /// </summary>
        /// <returns></returns>
        public string DayReport()
        {
            StringBuilder dayReportStr = new StringBuilder();
            dayReportStr.Append("\r\n");
            dayReportStr.Append(SetArgPosition(this.DayReportModel.StoreName + " " + this.DayReportModel.Title, this.TicketWidth, true));
            dayReportStr.Append(SetArgPosition(this.dayReportModel.Title2, this.TicketWidth, true));
            dayReportStr.Append("\r\n");
            dayReportStr.Append(SetArgPosition(this.dayReportModel.StockDateStr, this.TicketWidth, false));
            dayReportStr.Append(SetArgPosition("打印时间：" + this.DayReportModel.PrintDate.ToString("yyyy-MM-dd HH:mm:ss"), this.TicketWidth, false));
            dayReportStr.Append(CreateLine(this.TicketWidth, this.signWeight));
            dayReportStr.Append(ArrangeArgPosition("交易", this.TicketWidth, this.Colper1));
            dayReportStr.Append(ArrangeArgPosition("笔数", this.TicketWidth, this.Colper2, true));
            dayReportStr.Append(ArrangeArgPosition("金额", this.TicketWidth, this.Colper3, true));
            dayReportStr.Append("\r\n");
            dayReportStr.Append(CreateLine(this.TicketWidth, this.signWeight));
            if (this.DayReportModel.TransactionItemList != null)
            {
                foreach (var transactionItem in this.dayReportModel.TransactionItemList)
                {
                    int charCount = System.Text.Encoding.Default.GetByteCount(transactionItem.TransactionName);
                    StringBuilder name = new StringBuilder();
                    for (var i = 0; i < 8 - charCount; i++)
                    {
                        name.Append(" ");
                    }
                    name.Append(transactionItem.TransactionName);
                    dayReportStr.Append(ArrangeArgPosition(name.ToString() + ":", this.TicketWidth, this.Colper1));
                    dayReportStr.Append(ArrangeArgPosition(transactionItem.StrokeCount.ToString(), this.TicketWidth, this.Colper2, true));
                    dayReportStr.Append(ArrangeArgPosition(transactionItem.TotalAmount.ToString("N2"), this.TicketWidth, this.Colper3, true));
                    dayReportStr.Append("\r\n");
                }
                //dayReportStr.Append("\r\n说明：销售合计不含退换出款、\r\n换货入款、退单合计\r\n");
            }
            dayReportStr.Append(CreateLine(this.TicketWidth, this.signWeight));

            if (this.dayReportModel.EmployeeList != null)
            {
                foreach (var employee in this.dayReportModel.EmployeeList)
                {
                    dayReportStr.Append("\r\n");
                    dayReportStr.Append(SetArgPosition(employee.EmployeeSN + " " + employee.Name, this.TicketWidth, false));
                    if (employee.BeginTime >= DateTime.Parse("2015-01-01") && employee.EndTime >= DateTime.Parse("2015-01-01"))
                    {
                        dayReportStr.Append(SetArgPosition("首笔时间:" + employee.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"), this.TicketWidth, false));
                        dayReportStr.Append(SetArgPosition("末笔时间:" + employee.EndTime.ToString("yyyy-MM-dd HH:mm:ss"), this.TicketWidth, false));
                    }
                    dayReportStr.Append("\r\n");
                    if (employee.EmployeeTransactionItems != null)
                    {
                        foreach (var employeeTransactionItem in employee.EmployeeTransactionItems)
                        {
                            dayReportStr.Append(ArrangeArgPosition(employeeTransactionItem.TransactionName + ":", this.TicketWidth, this.Colper1));
                            if (employeeTransactionItem.StrokeCount == null)
                            {
                                dayReportStr.Append(ArrangeArgPosition(" ", this.TicketWidth, this.Colper2, true));
                            }
                            else
                            {
                                dayReportStr.Append(ArrangeArgPosition(employeeTransactionItem.StrokeCount.ToString(), this.TicketWidth, this.Colper2, true));
                            }
                            dayReportStr.Append(ArrangeArgPosition(employeeTransactionItem.TotalAmount.ToString("N2"), this.TicketWidth, this.Colper3, true));
                            dayReportStr.Append("\r\n");
                            if (employeeTransactionItem.ChildItems != null)
                            {
                                foreach (var childItem in employeeTransactionItem.ChildItems)
                                {
                                    dayReportStr.Append(ArrangeArgPosition("  " + childItem.Key + ":", this.TicketWidth, this.Colper1 + 0.10M));
                                    if (childItem.Key.Trim().Contains("自动抹零"))
                                    {
                                        var tempValue = childItem.Value.ToString("0.00#") + "（不记入收款）";
                                        if (tempValue.Length > 9)
                                        {
                                            tempValue = tempValue.Substring(0, 9) + "\n" + tempValue.Substring(9, tempValue.Length - 9);
                                        }

                                        dayReportStr.Append(ArrangeArgPosition(tempValue, this.TicketWidth, 0.9M - this.Colper1));
                                    }
                                    else if (childItem.Key.Trim().Contains("整单让利"))
                                    {
                                        var tempValue = childItem.Value.ToString("N2") + "（不记入收款）";
                                        if (tempValue.Length > 9)
                                        {
                                            tempValue = tempValue.Substring(0, 9) + "\n" + tempValue.Substring(9, tempValue.Length - 9);
                                        }

                                        dayReportStr.Append(ArrangeArgPosition(tempValue, this.TicketWidth, 0.9M - this.Colper1));
                                    }
                                    else
                                    {
                                        dayReportStr.Append(ArrangeArgPosition(childItem.Value.ToString("N2"), this.TicketWidth, 0.9M - this.Colper1));
                                    }
                                    dayReportStr.Append("\r\n");
                                }
                            }

                        }
                    }

                    dayReportStr.Append("\r\n");
                    dayReportStr.Append(CreateLine(this.TicketWidth, this.signWeight));
                }
            }

            dayReportStr.Append("\r\n");
            // dayReportStr.Append(CreateLine(this.TicketWidth, this.signWeight));

            return dayReportStr.ToString();
        }
    }
}
