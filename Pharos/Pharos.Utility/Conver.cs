﻿/*----------------------------------------------------------------
 * 功能描述：常规转换
 * 创 建 人：蔡少发
 * 创建时间：2015-05-11
//----------------------------------------------------------------*/

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Microsoft.International.Converters.PinYinConverter;

namespace Pharos.Utility
{
    /// <summary>
    /// 常规转换
    /// </summary>
    public static class Conver
    {
        #region 将上传文件转换为流

        /// <summary>
        /// 将上传文件转换为流
        /// </summary>
        /// <param name="file">HtmlInputFile 控件名称</param>
        /// <returns>返回流</returns>
        public static byte[] ToByteForFile(FileUpload file)
        {
            int fileLength = file.PostedFile.ContentLength;
            byte[] input = new byte[fileLength];

            using (Stream stream = file.PostedFile.InputStream)
            {
                stream.Read(input, 0, fileLength);
                stream.Position = 0;
                stream.Dispose();
                stream.Close();
            }

            return input;
        }

        /// <summary>
        /// 将上传文件转换为流
        /// </summary>
        /// <param name="file">HtmlInputFile 控件名称</param>
        /// <returns>返回流</returns>
        public static byte[] ToByteForFile(HtmlInputFile file)
        {
            int fileLength = file.PostedFile.ContentLength;
            byte[] input = new byte[fileLength];

            using (Stream stream = file.PostedFile.InputStream)
            {
                stream.Read(input, 0, fileLength);
                stream.Position = 0;
                stream.Dispose();
                stream.Close();
            }

            return input;
        }

        #endregion

        #region 将URL中的限定字符转为可识别的URL

        /// <summary>
        /// 将URL中的限定字符转为可识别的URL
        /// 
        /// <![CDATA[
        /// 
        /// 如：将 # 号转换为 %23，\ 转为 /
        /// 
        /// ]]>
        /// </summary>
        /// <param name="url">要转换的URL</param>
        /// <returns>返回可识别的URL</returns>
        public static string ToValidUrl(this string url)
        {
            return url.Replace(@"\", @"/").Replace("#", "%23");
        }

        #endregion

        #region 将Path中的限定字符转为可识别的Path

        /// <summary>
        /// 将Path中的限定字符转为可识别的Path
        /// 
        /// <![CDATA[
        /// 
        /// 如：将 D:\a/b\c 转为 D:\a\b\c
        /// 
        /// ]]>
        /// </summary>
        /// <param name="url">要转换的URL</param>
        /// <returns>返回可识别的URL</returns>
        public static string ToValidPath(this string url)
        {
            return url.Replace(@"/", @"\");
        }

        #endregion

        #region UTF-8 与 GB2312 编码互转

        /// <summary>
        /// UTF-8 与 GB2312 编码互转
        /// </summary>
        /// <param name="content">要转换的内容</param>
        /// <param name="outType">要输出的编码类型（枚举）</param>
        public static string UrlEncode(this string content, EncodeType outType)
        {
            if (string.IsNullOrEmpty(content)) { return string.Empty; }

            switch (outType)
            {
                case EncodeType.GB2312:
                    HttpContext.Current.Session.CodePage = (int)EncodeType.UTF8;
                    content = HttpContext.Current.Server.UrlEncode(content);
                    break;

                case EncodeType.UTF8:
                    HttpContext.Current.Session.CodePage = (int)EncodeType.GB2312;
                    content = HttpContext.Current.Server.UrlEncode(content);
                    break;
            }
            HttpContext.Current.Session.CodePage = (int)outType;

            return content;
        }

        #endregion

        #region 对 UrlEncode 进行解编码

        /// <summary>
        /// 对 UrlEncode 进行解码
        /// </summary>
        /// <param name="content">编码后的内容</param>
        public static string ToUrlDecode(this string content)
        {
            return HttpContext.Current.Server.UrlDecode(content);
        }

        /// <summary>
        /// 用 UrlEncode 进行编码
        /// </summary>
        /// <param name="content">需要编码的内容</param>
        public static string ToUrlEncode(this string content)
        {
            return HttpContext.Current.Server.UrlEncode(content);
        }

        /// <summary>
        /// 用 UrlEncode 进行编码
        /// </summary>
        /// <param name="content">需要编码的内容</param>
        public static string ToUrlEncodeUnicode(this string content)
        {
            return HttpUtility.UrlEncodeUnicode(content);
        }

        #endregion

        #region ASCII 或 Unicode 编码字符的 byte[] 与 String 互转

        /// <summary>
        /// 将ASCII编码字符的byte[]数组转化为 String
        /// </summary>
        /// <param name="byteArr">byte[]数组</param>
        public static string ASCIIbyteArrayToString(this byte[] byteArr)
        {
            return new ASCIIEncoding().GetString(byteArr);
        }

        /// <summary>
        /// 将String转化为ASCII编码字符的byte[]数组
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        public static byte[] StringToASCIIbyte(this string str)
        {
            return new ASCIIEncoding().GetBytes(str);
        }

        /// <summary>
        /// 将Unicode编码字符的byte[]数组转化为 String
        /// </summary>
        /// <param name="byteArr">byte[]数组</param>
        public static string UnicodeByteArrayToString(this byte[] byteArr)
        {
            return new UnicodeEncoding().GetString(byteArr);
        }

        /// <summary>
        /// 将String转化为Unicode编码字符的byte[]数组
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        public static byte[] StringToUnicodeByteArray(this string str)
        {
            return new UnicodeEncoding().GetBytes(str);
        }

        #endregion

        #region 仅将全角数字转换为半角

        /// <summary>
        /// 仅将全角数字转换为半角
        /// </summary>
        /// <param name="content">要转换的内容</param>
        public static string NumToDBC(this string content)
        {
            return content.Replace("０", "0").Replace("１", "1").Replace("２", "2").Replace("３", "3").Replace("４", "4")
                          .Replace("５", "5").Replace("６", "6").Replace("７", "7").Replace("８", "8").Replace("９", "9");
        }

        #endregion

        #region 仅将半角数字转换为全角

        /// <summary>
        /// 仅将半角数字转换为全角
        /// </summary>
        /// <param name="content">要转换的内容</param>
        public static string NumToSBC(this string content)
        {
            return content.Replace("0", "０").Replace("1", "１").Replace("2", "２").Replace("3", "３").Replace("4", "４")
                          .Replace("5", "５").Replace("6", "６").Replace("7", "７").Replace("8", "８").Replace("9", "９");
        }

        #endregion

        #region 将小写金额转为大写金额

        /// <summary> 
        /// 转换数字金额主函数（包括小数） 
        /// </summary> 
        /// <param name="str">数字字符串</param> 
        public static string ConvertSum(string str)
        {
            decimal dec;
            if (!decimal.TryParse(str, out dec))
            {
                return "提示：仅支持数字类型！";
            }
            if (dec < 0)
            {
                return "提示：不支持负数！";
            }
            if (double.Parse(str) > 999999999999.99)
            {
                return "提示：数字太大，无法换算，只支持到一万亿元以下的金额！";
            }

            char[] ch = new char[1];
            ch[0] = '.'; 
            string[] splitstr = null;  
            splitstr = str.Split(ch[0]);

            if (splitstr.Length == 1)
            {
                return ConvertData(str) + "圆整";
            }
            else
            {
                string rstr;
                rstr = ConvertData(splitstr[0]) + "圆";
                rstr += ConvertXiaoShu(splitstr[1]);
                return rstr;
            }
        }

        /// <summary> 
        /// 转换数字（整数） 
        /// </summary> 
        /// <param name="str">需要转换的整数数字字符串</param> 
        /// <returns>转换成中文大写后的字符串</returns> 
        private static string ConvertData(string str)
        {
            string tmpstr = "";
            string rstr = "";
            int strlen = str.Length;

            if (strlen <= 4)
            {
                rstr = ConvertDigit(str);
            }
            else
            {
                if (strlen <= 8)
                {
                    tmpstr = str.Substring(strlen - 4, 4);
                    rstr = ConvertDigit(tmpstr); 
                    tmpstr = str.Substring(0, strlen - 4); 

                    rstr = String.Concat(ConvertDigit(tmpstr) + "萬", rstr);
                    rstr = rstr.Replace("零萬", "萬").Replace("零零", "零");
                }
                else
                {
                    if (strlen <= 12)
                    {
                        tmpstr = str.Substring(strlen - 4, 4);
                        rstr = ConvertDigit(tmpstr); 
                        tmpstr = str.Substring(strlen - 8, 4);
                        rstr = String.Concat(ConvertDigit(tmpstr) + "萬", rstr);
                        tmpstr = str.Substring(0, strlen - 8);
                        rstr = String.Concat(ConvertDigit(tmpstr) + "億", rstr);
                        rstr = rstr.Replace("零億", "億").Replace("零萬", "零").Replace("零零", "零");
                    }
                }
            }
            strlen = rstr.Length;
            if (strlen >= 2)
            {
                switch (rstr.Substring(strlen - 2, 2))
                {
                    case "佰零": rstr = rstr.Substring(0, strlen - 2) + "佰"; break;
                    case "仟零": rstr = rstr.Substring(0, strlen - 2) + "仟"; break;
                    case "萬零": rstr = rstr.Substring(0, strlen - 2) + "萬"; break;
                    case "億零": rstr = rstr.Substring(0, strlen - 2) + "億"; break;

                }
            }

            return rstr;
        }
        /// <summary> 
        /// 转换数字（小数部分） 
        /// </summary> 
        /// <param name="str">需要转换的小数部分数字字符串</param> 
        /// <returns>转换成中文大写后的字符串</returns> 
        private static string ConvertXiaoShu(string str)
        {
            int strlen = str.Length;
            string rstr;

            if (strlen == 1)
            {
                rstr = ConvertChinese(str) + "角";
                return rstr;
            }
            else
            {
                string tmpstr = str.Substring(0, 1);
                rstr = ConvertChinese(tmpstr) + "角";
                tmpstr = str.Substring(1, 1);
                rstr += ConvertChinese(tmpstr) + "分";
                rstr = rstr.Replace("零分", "").Replace("零角", "");
                return rstr;
            }
        }

        /// <summary> 
        /// 转换数字 
        /// </summary> 
        /// <param name="str">转换的字符串（四位以内）</param> 
        private static string ConvertDigit(string str)
        {
            int strlen = str.Length;
            string rstr = "";

            switch (strlen)
            {
                case 1: rstr = ConvertChinese(str); break;
                case 2: rstr = Convert2Digit(str); break;
                case 3: rstr = Convert3Digit(str); break;
                case 4: rstr = Convert4Digit(str); break;
            }
            rstr = rstr.Replace("拾零", "拾");
            strlen = rstr.Length;

            return rstr;
        }

        /// <summary> 
        /// 转换四位数字 
        /// </summary> 
        private static string Convert4Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);
            string str4 = str.Substring(3, 1);

            string rstring = "";
            rstring += ConvertChinese(str1) + "仟";
            rstring += ConvertChinese(str2) + "佰";
            rstring += ConvertChinese(str3) + "拾";
            rstring += ConvertChinese(str4);

            rstring = rstring.Replace("零仟", "零").Replace("零佰", "零").Replace("零拾", "零").Replace("零零", "零");
            return rstring;
        }

        /// <summary> 
        /// 转换三位数字 
        /// </summary> 
        private static string Convert3Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);

            string rstring = "";
            rstring += ConvertChinese(str1) + "佰";
            rstring += ConvertChinese(str2) + "拾";
            rstring += ConvertChinese(str3);

            rstring = rstring.Replace("零佰", "零").Replace("零拾", "零").Replace("零零", "零");
            return rstring;
        }

        /// <summary> 
        /// 转换二位数字 
        /// </summary> 
        private static string Convert2Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);

            string rstring = "";
            rstring += ConvertChinese(str1) + "拾";
            rstring += ConvertChinese(str2);

            rstring = rstring.Replace("零拾", "零").Replace("零零", "零");
            return rstring;
        }

        /// <summary> 
        /// 将一位数字转换成中文大写数字 
        /// </summary> 
        private static string ConvertChinese(string str)
        {
            //"零壹贰叁肆伍陆柒捌玖拾佰仟萬億圆整角分" 
            string cstr = "";
            switch (str)
            {
                case "0": cstr = "零"; break;
                case "1": cstr = "壹"; break;
                case "2": cstr = "贰"; break;
                case "3": cstr = "叁"; break;
                case "4": cstr = "肆"; break;
                case "5": cstr = "伍"; break;
                case "6": cstr = "陆"; break;
                case "7": cstr = "柒"; break;
                case "8": cstr = "捌"; break;
                case "9": cstr = "玖"; break;
            }
            return (cstr);
        }

        #endregion

        #region 简体与繁体字互转

        /// <summary>
        /// 将繁体转为简体
        /// </summary>
        /// <param name="str">要转换的繁体</param>
        /// <returns>转后的简体</returns>
        public static string TraditionalToSimplified(this string str)
        {
            return (Strings.StrConv(str, VbStrConv.SimplifiedChinese, 0));
        }

        /// <summary>
        /// 将简体转为繁体
        /// </summary>
        /// <param name="str">要转换的简体</param>
        /// <returns>转后的繁体</returns>
        public static string SimplifiedToTraditional(this string str)
        {
            return (Strings.StrConv(str as String, VbStrConv.TraditionalChinese, 0));
        }

        #endregion

        #region 转换为全角

        /// <summary>
        /// 转换为全角
        /// 
        /// <![CDATA[
        /// 
        /// 全角空格为12288，半角空格为32，
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248。
        /// 
        /// ]]>
        /// </summary>
        /// <param name="content">要转的内容</param>
        public static string ToSBC(this string content)
        {
            char[] c = content.ToCharArray();
            int i = 0;
            int len = c.Length;
            for (i = 0; i < len; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                {
                    c[i] = (char)(c[i] + 65248);
                }
            }
            return new string(c);
        }

        #endregion

        #region 转换为半角

        /// <summary>
        /// 转换为半角
        /// 
        /// <![CDATA[
        /// 
        /// 全角空格为12288，半角空格为32，
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248。
        /// 
        /// ]]>
        /// </summary>
        /// <param name="content">要转的内容</param>
        public static string ToDBC(this string content)
        {
            char[] c = content.ToCharArray();
            int i = 0;
            int len = c.Length;
            for (i = 0; i < len; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] >= 65281 && c[i] <= 65373)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }
            return new string(c);
        }

        #endregion

        #region 简体汉字转拼音

        private static List<string> _py;
        private static List<int> _num;

        /// <summary>
        /// 简体汉字转拼音
        /// </summary>
        /// <param name="HanZi">要转换的汉字</param>
        /// <param name="py">拼音转换样式（枚举）</param>
        /// <param name="Separator">分隔符</param>
        public static string ToPinYin(this string HanZi, PYStyle py, string Separator)
        {
            if (string.IsNullOrEmpty(HanZi)) { return string.Empty; }

            StringBuilder sb = new StringBuilder();
            Encoding encd = Encoding.GetEncoding("gb2312");

            int length = HanZi.Length;

            PYInit();

            for (int i = 0; i < length; i++)
            {
                int n = Convert.ToInt32(HanZi[i]);
                if (n >= 0x4e00 && n <= 0x9fa5)
                {
                    byte[] a = encd.GetBytes(HanZi[i].ToString());
                    n = (short)a[0] * 256 + (short)a[1] - 65536;

                    int j = _num.Count - 1;

                    while (j >= 0 && _num[j] > n)
                    {
                        j--;
                    }

                    if (j >= 0)
                    {
                        switch (py)
                        {
                            case PYStyle.First_Lower:
                                sb.Append(_py[j].Substring(0, 1).ToLower());
                                break;

                            case PYStyle.First_Upper:
                                sb.Append(_py[j].Substring(0, 1).ToUpper());
                                break;

                            case PYStyle.Full_Lower:
                                sb.Append(_py[j].ToLower());
                                break;

                            case PYStyle.Full_Upper:
                                sb.Append(_py[j].ToUpper());
                                break;

                            case PYStyle.Full_FirstUpper:
                                sb.Append(_py[j].Substring(0, 1).ToUpper()).Append(_py[j].Substring(1).ToLower());
                                break;

                            default:
                                sb.Append(_py[j]);
                                break;
                        }
                        sb.Append(Separator);
                    }
                }
                else
                {
                    sb.Append(HanZi[i]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 简体汉字转拼音
        /// </summary>
        /// <param name="HanZi">要转换的汉字</param>
        public static string ToPinYin(this string HanZi)
        {
            return ToPinYin(HanZi, PYStyle.Full_FirstUpper, string.Empty);
        }

        /// <summary>
        /// 简体汉字转拼音
        /// </summary>
        /// <param name="HanZi">要转换的汉字</param>
        /// <param name="Separator">分隔符</param>
        public static string ToPinYin(this string HanZi, string Separator)
        {
            return ToPinYin(HanZi, PYStyle.Full_FirstUpper, Separator);
        }

        private static void PYInit()
        {
            _py = new List<string>();
            _num = new List<int>();

            _py.Add("a"); _num.Add(-20319);
            _py.Add("ai"); _num.Add(-20317);
            _py.Add("an"); _num.Add(-20304);
            _py.Add("ang"); _num.Add(-20295);
            _py.Add("ao"); _num.Add(-20292);
            _py.Add("ba"); _num.Add(-20283);
            _py.Add("bai"); _num.Add(-20265);
            _py.Add("ban"); _num.Add(-20257);
            _py.Add("bang"); _num.Add(-20242);
            _py.Add("bao"); _num.Add(-20230);
            _py.Add("bei"); _num.Add(-20051);
            _py.Add("ben"); _num.Add(-20036);
            _py.Add("beng"); _num.Add(-20032);
            _py.Add("bi"); _num.Add(-20026);
            _py.Add("bian"); _num.Add(-20002);
            _py.Add("biao"); _num.Add(-19990);
            _py.Add("bie"); _num.Add(-19986);
            _py.Add("bin"); _num.Add(-19982);
            _py.Add("bing"); _num.Add(-19976);
            _py.Add("bo"); _num.Add(-19805);
            _py.Add("bu"); _num.Add(-19784);
            _py.Add("ca"); _num.Add(-19775);
            _py.Add("cai"); _num.Add(-19774);
            _py.Add("can"); _num.Add(-19763);
            _py.Add("cang"); _num.Add(-19756);
            _py.Add("cao"); _num.Add(-19751);
            _py.Add("ce"); _num.Add(-19746);
            _py.Add("ceng"); _num.Add(-19741);
            _py.Add("cha"); _num.Add(-19739);
            _py.Add("chai"); _num.Add(-19728);
            _py.Add("chan"); _num.Add(-19725);
            _py.Add("chang"); _num.Add(-19715);
            _py.Add("chao"); _num.Add(-19540);
            _py.Add("che"); _num.Add(-19531);
            _py.Add("chen"); _num.Add(-19525);
            _py.Add("cheng"); _num.Add(-19515);
            _py.Add("chi"); _num.Add(-19500);
            _py.Add("chong"); _num.Add(-19484);
            _py.Add("chou"); _num.Add(-19479);
            _py.Add("chu"); _num.Add(-19467);
            _py.Add("chuai"); _num.Add(-19289);
            _py.Add("chuan"); _num.Add(-19288);
            _py.Add("chuang"); _num.Add(-19281);
            _py.Add("chui"); _num.Add(-19275);
            _py.Add("chun"); _num.Add(-19270);
            _py.Add("chuo"); _num.Add(-19263);
            _py.Add("ci"); _num.Add(-19261);
            _py.Add("cong"); _num.Add(-19249);
            _py.Add("cou"); _num.Add(-19243);
            _py.Add("cu"); _num.Add(-19242);
            _py.Add("cuan"); _num.Add(-19238);
            _py.Add("cui"); _num.Add(-19235);
            _py.Add("cun"); _num.Add(-19227);
            _py.Add("cuo"); _num.Add(-19224);
            _py.Add("da"); _num.Add(-19218);
            _py.Add("dai"); _num.Add(-19212);
            _py.Add("dan"); _num.Add(-19038);
            _py.Add("dang"); _num.Add(-19023);
            _py.Add("dao"); _num.Add(-19018);
            _py.Add("de"); _num.Add(-19006);
            _py.Add("deng"); _num.Add(-19003);
            _py.Add("di"); _num.Add(-18996);
            _py.Add("dian"); _num.Add(-18977);
            _py.Add("diao"); _num.Add(-18961);
            _py.Add("die"); _num.Add(-18952);
            _py.Add("ding"); _num.Add(-18783);
            _py.Add("diu"); _num.Add(-18774);
            _py.Add("dong"); _num.Add(-18773);
            _py.Add("dou"); _num.Add(-18763);
            _py.Add("du"); _num.Add(-18756);
            _py.Add("duan"); _num.Add(-18741);
            _py.Add("dui"); _num.Add(-18735);
            _py.Add("dun"); _num.Add(-18731);
            _py.Add("duo"); _num.Add(-18722);
            _py.Add("e"); _num.Add(-18710);
            _py.Add("en"); _num.Add(-18697);
            _py.Add("er"); _num.Add(-18696);
            _py.Add("fa"); _num.Add(-18526);
            _py.Add("fan"); _num.Add(-18518);
            _py.Add("fang"); _num.Add(-18501);
            _py.Add("fei"); _num.Add(-18490);
            _py.Add("fen"); _num.Add(-18478);
            _py.Add("feng"); _num.Add(-18463);
            _py.Add("fo"); _num.Add(-18448);
            _py.Add("fou"); _num.Add(-18447);
            _py.Add("fu"); _num.Add(-18446);
            _py.Add("ga"); _num.Add(-18239);
            _py.Add("gai"); _num.Add(-18237);
            _py.Add("gan"); _num.Add(-18231);
            _py.Add("gang"); _num.Add(-18220);
            _py.Add("gao"); _num.Add(-18211);
            _py.Add("ge"); _num.Add(-18201);
            _py.Add("gei"); _num.Add(-18184);
            _py.Add("gen"); _num.Add(-18183);
            _py.Add("geng"); _num.Add(-18181);
            _py.Add("gong"); _num.Add(-18012);
            _py.Add("gou"); _num.Add(-17997);
            _py.Add("gu"); _num.Add(-17988);
            _py.Add("gua"); _num.Add(-17970);
            _py.Add("guai"); _num.Add(-17964);
            _py.Add("guan"); _num.Add(-17961);
            _py.Add("guang"); _num.Add(-17950);
            _py.Add("gui"); _num.Add(-17947);
            _py.Add("gun"); _num.Add(-17931);
            _py.Add("guo"); _num.Add(-17928);
            _py.Add("ha"); _num.Add(-17922);
            _py.Add("hai"); _num.Add(-17759);
            _py.Add("han"); _num.Add(-17752);
            _py.Add("hang"); _num.Add(-17733);
            _py.Add("hao"); _num.Add(-17730);
            _py.Add("he"); _num.Add(-17721);
            _py.Add("hei"); _num.Add(-17703);
            _py.Add("hen"); _num.Add(-17701);
            _py.Add("heng"); _num.Add(-17697);
            _py.Add("hong"); _num.Add(-17692);
            _py.Add("hou"); _num.Add(-17683);
            _py.Add("hu"); _num.Add(-17676);
            _py.Add("hua"); _num.Add(-17496);
            _py.Add("huai"); _num.Add(-17487);
            _py.Add("huan"); _num.Add(-17482);
            _py.Add("huang"); _num.Add(-17468);
            _py.Add("hui"); _num.Add(-17454);
            _py.Add("hun"); _num.Add(-17433);
            _py.Add("huo"); _num.Add(-17427);
            _py.Add("ji"); _num.Add(-17417);
            _py.Add("jia"); _num.Add(-17202);
            _py.Add("jian"); _num.Add(-17185);
            _py.Add("jiang"); _num.Add(-16983);
            _py.Add("jiao"); _num.Add(-16970);
            _py.Add("jie"); _num.Add(-16942);
            _py.Add("jin"); _num.Add(-16915);
            _py.Add("jing"); _num.Add(-16733);
            _py.Add("jiong"); _num.Add(-16708);
            _py.Add("jiu"); _num.Add(-16706);
            _py.Add("ju"); _num.Add(-16689);
            _py.Add("juan"); _num.Add(-16664);
            _py.Add("jue"); _num.Add(-16657);
            _py.Add("jun"); _num.Add(-16647);
            _py.Add("ka"); _num.Add(-16474);
            _py.Add("kai"); _num.Add(-16470);
            _py.Add("kan"); _num.Add(-16465);
            _py.Add("kang"); _num.Add(-16459);
            _py.Add("kao"); _num.Add(-16452);
            _py.Add("ke"); _num.Add(-16448);
            _py.Add("ken"); _num.Add(-16433);
            _py.Add("keng"); _num.Add(-16429);
            _py.Add("kong"); _num.Add(-16427);
            _py.Add("kou"); _num.Add(-16423);
            _py.Add("ku"); _num.Add(-16419);
            _py.Add("kua"); _num.Add(-16412);
            _py.Add("kuai"); _num.Add(-16407);
            _py.Add("kuan"); _num.Add(-16403);
            _py.Add("kuang"); _num.Add(-16401);
            _py.Add("kui"); _num.Add(-16393);
            _py.Add("kun"); _num.Add(-16220);
            _py.Add("kuo"); _num.Add(-16216);
            _py.Add("la"); _num.Add(-16212);
            _py.Add("lai"); _num.Add(-16205);
            _py.Add("lan"); _num.Add(-16202);
            _py.Add("lang"); _num.Add(-16187);
            _py.Add("lao"); _num.Add(-16180);
            _py.Add("le"); _num.Add(-16171);
            _py.Add("lei"); _num.Add(-16169);
            _py.Add("leng"); _num.Add(-16158);
            _py.Add("li"); _num.Add(-16155);
            _py.Add("lia"); _num.Add(-15959);
            _py.Add("lian"); _num.Add(-15958);
            _py.Add("liang"); _num.Add(-15944);
            _py.Add("liao"); _num.Add(-15933);
            _py.Add("lie"); _num.Add(-15920);
            _py.Add("lin"); _num.Add(-15915);
            _py.Add("ling"); _num.Add(-15903);
            _py.Add("liu"); _num.Add(-15889);
            _py.Add("long"); _num.Add(-15878);
            _py.Add("lou"); _num.Add(-15707);
            _py.Add("lu"); _num.Add(-15701);
            _py.Add("lv"); _num.Add(-15681);
            _py.Add("luan"); _num.Add(-15667);
            _py.Add("lue"); _num.Add(-15661);
            _py.Add("lun"); _num.Add(-15659);
            _py.Add("luo"); _num.Add(-15652);
            _py.Add("ma"); _num.Add(-15640);
            _py.Add("mai"); _num.Add(-15631);
            _py.Add("man"); _num.Add(-15625);
            _py.Add("mang"); _num.Add(-15454);
            _py.Add("mao"); _num.Add(-15448);
            _py.Add("me"); _num.Add(-15436);
            _py.Add("mei"); _num.Add(-15435);
            _py.Add("men"); _num.Add(-15419);
            _py.Add("meng"); _num.Add(-15416);
            _py.Add("mi"); _num.Add(-15408);
            _py.Add("mian"); _num.Add(-15394);
            _py.Add("miao"); _num.Add(-15385);
            _py.Add("mie"); _num.Add(-15377);
            _py.Add("min"); _num.Add(-15375);
            _py.Add("ming"); _num.Add(-15369);
            _py.Add("miu"); _num.Add(-15363);
            _py.Add("mo"); _num.Add(-15362);
            _py.Add("mou"); _num.Add(-15183);
            _py.Add("mu"); _num.Add(-15180);
            _py.Add("na"); _num.Add(-15165);
            _py.Add("nai"); _num.Add(-15158);
            _py.Add("nan"); _num.Add(-15153);
            _py.Add("nang"); _num.Add(-15150);
            _py.Add("nao"); _num.Add(-15149);
            _py.Add("ne"); _num.Add(-15144);
            _py.Add("nei"); _num.Add(-15143);
            _py.Add("nen"); _num.Add(-15141);
            _py.Add("neng"); _num.Add(-15140);
            _py.Add("ni"); _num.Add(-15139);
            _py.Add("nian"); _num.Add(-15128);
            _py.Add("niang"); _num.Add(-15121);
            _py.Add("niao"); _num.Add(-15119);
            _py.Add("nie"); _num.Add(-15117);
            _py.Add("nin"); _num.Add(-15110);
            _py.Add("ning"); _num.Add(-15109);
            _py.Add("niu"); _num.Add(-14941);
            _py.Add("nong"); _num.Add(-14937);
            _py.Add("nu"); _num.Add(-14933);
            _py.Add("nv"); _num.Add(-14930);
            _py.Add("nuan"); _num.Add(-14929);
            _py.Add("nue"); _num.Add(-14928);
            _py.Add("nuo"); _num.Add(-14926);
            _py.Add("o"); _num.Add(-14922);
            _py.Add("ou"); _num.Add(-14921);
            _py.Add("pa"); _num.Add(-14914);
            _py.Add("pai"); _num.Add(-14908);
            _py.Add("pan"); _num.Add(-14902);
            _py.Add("pang"); _num.Add(-14894);
            _py.Add("pao"); _num.Add(-14889);
            _py.Add("pei"); _num.Add(-14882);
            _py.Add("pen"); _num.Add(-14873);
            _py.Add("peng"); _num.Add(-14871);
            _py.Add("pi"); _num.Add(-14857);
            _py.Add("pian"); _num.Add(-14678);
            _py.Add("piao"); _num.Add(-14674);
            _py.Add("pie"); _num.Add(-14670);
            _py.Add("pin"); _num.Add(-14668);
            _py.Add("ping"); _num.Add(-14663);
            _py.Add("po"); _num.Add(-14654);
            _py.Add("pu"); _num.Add(-14645);
            _py.Add("qi"); _num.Add(-14630);
            _py.Add("qia"); _num.Add(-14594);
            _py.Add("qian"); _num.Add(-14429);
            _py.Add("qiang"); _num.Add(-14407);
            _py.Add("qiao"); _num.Add(-14399);
            _py.Add("qie"); _num.Add(-14384);
            _py.Add("qin"); _num.Add(-14379);
            _py.Add("qing"); _num.Add(-14368);
            _py.Add("qiong"); _num.Add(-14355);
            _py.Add("qiu"); _num.Add(-14353);
            _py.Add("qu"); _num.Add(-14345);
            _py.Add("quan"); _num.Add(-14170);
            _py.Add("que"); _num.Add(-14159);
            _py.Add("qun"); _num.Add(-14151);
            _py.Add("ran"); _num.Add(-14149);
            _py.Add("rang"); _num.Add(-14145);
            _py.Add("rao"); _num.Add(-14140);
            _py.Add("re"); _num.Add(-14137);
            _py.Add("ren"); _num.Add(-14135);
            _py.Add("reng"); _num.Add(-14125);
            _py.Add("ri"); _num.Add(-14123);
            _py.Add("rong"); _num.Add(-14122);
            _py.Add("rou"); _num.Add(-14112);
            _py.Add("ru"); _num.Add(-14109);
            _py.Add("ruan"); _num.Add(-14099);
            _py.Add("rui"); _num.Add(-14097);
            _py.Add("run"); _num.Add(-14094);
            _py.Add("ruo"); _num.Add(-14092);
            _py.Add("sa"); _num.Add(-14090);
            _py.Add("sai"); _num.Add(-14087);
            _py.Add("san"); _num.Add(-14083);
            _py.Add("sang"); _num.Add(-13917);
            _py.Add("sao"); _num.Add(-13914);
            _py.Add("se"); _num.Add(-13910);
            _py.Add("sen"); _num.Add(-13907);
            _py.Add("seng"); _num.Add(-13906);
            _py.Add("sha"); _num.Add(-13905);
            _py.Add("shai"); _num.Add(-13896);
            _py.Add("shan"); _num.Add(-13894);
            _py.Add("shang"); _num.Add(-13878);
            _py.Add("shao"); _num.Add(-13870);
            _py.Add("she"); _num.Add(-13859);
            _py.Add("shen"); _num.Add(-13847);
            _py.Add("sheng"); _num.Add(-13831);
            _py.Add("shi"); _num.Add(-13658);
            _py.Add("shou"); _num.Add(-13611);
            _py.Add("shu"); _num.Add(-13601);
            _py.Add("shua"); _num.Add(-13406);
            _py.Add("shuai"); _num.Add(-13404);
            _py.Add("shuan"); _num.Add(-13400);
            _py.Add("shuang"); _num.Add(-13398);
            _py.Add("shui"); _num.Add(-13395);
            _py.Add("shun"); _num.Add(-13391);
            _py.Add("shuo"); _num.Add(-13387);
            _py.Add("si"); _num.Add(-13383);
            _py.Add("song"); _num.Add(-13367);
            _py.Add("sou"); _num.Add(-13359);
            _py.Add("su"); _num.Add(-13356);
            _py.Add("suan"); _num.Add(-13343);
            _py.Add("sui"); _num.Add(-13340);
            _py.Add("sun"); _num.Add(-13329);
            _py.Add("suo"); _num.Add(-13326);
            _py.Add("ta"); _num.Add(-13318);
            _py.Add("tai"); _num.Add(-13147);
            _py.Add("tan"); _num.Add(-13138);
            _py.Add("tang"); _num.Add(-13120);
            _py.Add("tao"); _num.Add(-13107);
            _py.Add("te"); _num.Add(-13096);
            _py.Add("teng"); _num.Add(-13095);
            _py.Add("ti"); _num.Add(-13091);
            _py.Add("tian"); _num.Add(-13076);
            _py.Add("tiao"); _num.Add(-13068);
            _py.Add("tie"); _num.Add(-13063);
            _py.Add("ting"); _num.Add(-13060);
            _py.Add("tong"); _num.Add(-12888);
            _py.Add("tou"); _num.Add(-12875);
            _py.Add("tu"); _num.Add(-12871);
            _py.Add("tuan"); _num.Add(-12860);
            _py.Add("tui"); _num.Add(-12858);
            _py.Add("tun"); _num.Add(-12852);
            _py.Add("tuo"); _num.Add(-12849);
            _py.Add("wa"); _num.Add(-12838);
            _py.Add("wai"); _num.Add(-12831);
            _py.Add("wan"); _num.Add(-12829);
            _py.Add("wang"); _num.Add(-12812);
            _py.Add("wei"); _num.Add(-12802);
            _py.Add("wen"); _num.Add(-12607);
            _py.Add("weng"); _num.Add(-12597);
            _py.Add("wo"); _num.Add(-12594);
            _py.Add("wu"); _num.Add(-12585);
            _py.Add("xi"); _num.Add(-12556);
            _py.Add("xia"); _num.Add(-12359);
            _py.Add("xian"); _num.Add(-12346);
            _py.Add("xiang"); _num.Add(-12320);
            _py.Add("xiao"); _num.Add(-12300);
            _py.Add("xie"); _num.Add(-12120);
            _py.Add("xin"); _num.Add(-12099);
            _py.Add("xing"); _num.Add(-12089);
            _py.Add("xiong"); _num.Add(-12074);
            _py.Add("xiu"); _num.Add(-12067);
            _py.Add("xu"); _num.Add(-12058);
            _py.Add("xuan"); _num.Add(-12039);
            _py.Add("xue"); _num.Add(-11867);
            _py.Add("xun"); _num.Add(-11861);
            _py.Add("ya"); _num.Add(-11847);
            _py.Add("yan"); _num.Add(-11831);
            _py.Add("yang"); _num.Add(-11798);
            _py.Add("yao"); _num.Add(-11781);
            _py.Add("ye"); _num.Add(-11604);
            _py.Add("yi"); _num.Add(-11589);
            _py.Add("yin"); _num.Add(-11536);
            _py.Add("ying"); _num.Add(-11358);
            _py.Add("yo"); _num.Add(-11340);
            _py.Add("yong"); _num.Add(-11339);
            _py.Add("you"); _num.Add(-11324);
            _py.Add("yu"); _num.Add(-11303);
            _py.Add("yuan"); _num.Add(-11097);
            _py.Add("yue"); _num.Add(-11077);
            _py.Add("yun"); _num.Add(-11067);
            _py.Add("za"); _num.Add(-11055);
            _py.Add("zai"); _num.Add(-11052);
            _py.Add("zan"); _num.Add(-11045);
            _py.Add("zang"); _num.Add(-11041);
            _py.Add("zao"); _num.Add(-11038);
            _py.Add("ze"); _num.Add(-11024);
            _py.Add("zei"); _num.Add(-11020);
            _py.Add("zen"); _num.Add(-11019);
            _py.Add("zeng"); _num.Add(-11018);
            _py.Add("zha"); _num.Add(-11014);
            _py.Add("zhai"); _num.Add(-10838);
            _py.Add("zhan"); _num.Add(-10832);
            _py.Add("zhang"); _num.Add(-10815);
            _py.Add("zhao"); _num.Add(-10800);
            _py.Add("zhe"); _num.Add(-10790);
            _py.Add("zhen"); _num.Add(-10780);
            _py.Add("zheng"); _num.Add(-10764);
            _py.Add("zhi"); _num.Add(-10587);
            _py.Add("zhong"); _num.Add(-10544);
            _py.Add("zhou"); _num.Add(-10533);
            _py.Add("zhu"); _num.Add(-10519);
            _py.Add("zhua"); _num.Add(-10331);
            _py.Add("zhuai"); _num.Add(-10329);
            _py.Add("zhuan"); _num.Add(-10328);
            _py.Add("zhuang"); _num.Add(-10322);
            _py.Add("zhui"); _num.Add(-10315);
            _py.Add("zhun"); _num.Add(-10309);
            _py.Add("zhuo"); _num.Add(-10307);
            _py.Add("zi"); _num.Add(-10296);
            _py.Add("zong"); _num.Add(-10281);
            _py.Add("zou"); _num.Add(-10274);
            _py.Add("zu"); _num.Add(-10270);
            _py.Add("zuan"); _num.Add(-10262);
            _py.Add("zui"); _num.Add(-10260);
            _py.Add("zun"); _num.Add(-10256);
            _py.Add("zuo"); _num.Add(-10254);
            _py.Add("zhen"); _num.Add(-9254); // 修正:圳
        }

        #endregion

        #region 拼音转换类型枚举

        /// <summary>
        /// 拼音转换样式
        /// </summary>
        public enum PYStyle
        {
            /// <summary>
            /// 只取首字母，小写
            /// </summary>
            First_Lower,

            /// <summary>
            /// 只取首字母，大写
            /// </summary>
            First_Upper,

            /// <summary>
            /// 全拼，小写
            /// </summary>
            Full_Lower,

            /// <summary>
            /// 全拼，大写
            /// </summary>
            Full_Upper,

            /// <summary>
            /// 全拼，首字母大写
            /// </summary>
            Full_FirstUpper
        }

        /// <summary>
        /// 编码类型
        /// </summary>
        public enum EncodeType
        {
            /// <summary>
            /// UTF-8
            /// </summary>
            UTF8 = 65001,
            /// <summary>
            /// GB-2312
            /// </summary>
            GB2312 = 936
        }

        #endregion

        #region 取汉字首字母

        /// <summary>
        /// 取汉字首字母
        /// </summary>
        /// <param name="text">汉字</param>
        public static string GetFirstLetter(this string text)
        {
            int len = text.Length;
            string mystr = string.Empty;

            for (int i = 0; i < len; i++)
            {
                mystr += GetLetter(text.Substring(i, 1));
            }

            return mystr;
        }
        private static string GetLetter(string cnChar)
        {
            byte[] arrCn = Encoding.Default.GetBytes(cnChar);

            if (arrCn.Length > 1)
            {
                int area = (short)arrCn[0];
                int pos = (short)arrCn[1];
                int code = (area << 8) + pos;
                int[] areaCode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };

                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25)
                    {
                        max = areaCode[i + 1];
                    }

                    if (areaCode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }

            return cnChar;
        }

        #endregion
        #region  汉字转换拼音

        /// <summary>
        /// 汉字转换拼音（全拼）
        /// </summary>
        /// <param name="txt">汉字</param>
        /// <returns>拼音首个字母大写的全拼</returns>
        public static string GetFullSpell(this string txt)
        {
            if (txt == null) return "";
            txt = txt.Trim();
            byte[] arr = new byte[2]; //每个汉字为2字节  
            StringBuilder result = new StringBuilder();//使用StringBuilder优化字符串连接              
            char[] arrChar = txt.ToCharArray();
            foreach (char c in arrChar)
            {
                arr = System.Text.Encoding.Default.GetBytes(c.ToString());//根据系统默认编码得到字节码  
                if (arr.Length == 1)//如果只有1字节说明该字符不是汉字                  
                {
                    result.Append(c.ToString());
                    continue;
                }
                ChineseChar chineseChar = new ChineseChar(c);
                string p = chineseChar.Pinyins[0];
                result.Append(p.Substring(0, 1).ToUpper());
                result.Append(p.Substring(1, p.Length - 2).ToLower());
            }
            return result.ToString();
        }

        #endregion

        #region 产品数量格式转换
        public static string ProductNumberToStr(decimal productNumber)
        {
            if (productNumber == (int)productNumber)
            {
                return productNumber.ToString("f0");
            }
            else if (decimal.Parse(productNumber.ToString("f1")) == productNumber)
            {
                return productNumber.ToString("f1");
            }              
                
            return productNumber.ToString("f2");
        }
        #endregion

        #region 称重量为0时去除单位
        public static string FormatterZeroWeigh(string weigh)
        {
            string tempWeigh = weigh.Trim().ToUpper();
            if(tempWeigh == "0KG" || tempWeigh == "0公斤" || tempWeigh == "0G" || tempWeigh == "0克" || tempWeigh == "0斤")
            {
                return "0";
            }
            return weigh;
        }

        #endregion
    }
}
