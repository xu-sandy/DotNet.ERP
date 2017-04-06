using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace  Pharos.POS.Retailing.Models.Printer
{
    /// <summary>
    /// LPTControl 的摘要说明。
    /// </summary>
    public class LptControl
    {
        private string LptStr = "lpt1";
        public LptControl(string l_LPT_Str)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            LptStr = l_LPT_Str;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct OVERLAPPED
        {
            int Internal;
            int InternalHigh;
            int Offset;
            int OffSetHigh;
            int hEvent;
        }
        [DllImport("kernel32.dll")]
        private static extern int CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode, int lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);
        [DllImport("kernel32.dll")]
        private static extern bool WriteFile(int hFile, byte[] lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, ref OVERLAPPED lpOverlapped);
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(int hObject);
        private int iHandle;
        public bool Open()
        {
            iHandle = CreateFile(LptStr, 0x40000000, 0, 0, 3, 0, 0);
            // iHandle = CreateFile(LptStr, GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0);

            if (iHandle != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Write(String Mystring)
        {
            if (iHandle != -1)
            {
                OVERLAPPED x = new OVERLAPPED();
                int i = 0;
                byte[] mybyte = System.Text.Encoding.Default.GetBytes(Mystring);
                bool b = WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
                return b;
            }
            else
            {
                throw new Exception("不能连接到打印机!");
            }
        }
        public bool Write(byte[] mybyte)
        {
            if (iHandle != -1)
            {
                OVERLAPPED x = new OVERLAPPED();
                int i = 0;
                return WriteFile(iHandle, mybyte, mybyte.Length, ref i, ref x);
            }
            else
            {
                throw new Exception("不能连接到打印机!");
            }
        }


        public bool Close()
        {
            return CloseHandle(iHandle);
        }

    }
 } 
 
