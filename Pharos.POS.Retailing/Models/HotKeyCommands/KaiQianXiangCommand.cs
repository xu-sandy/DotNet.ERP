using Pharos.Wpf.HotKeyHelper;
using System;
using System.Drawing.Printing;
using System.Windows;

namespace Pharos.POS.Retailing.Models.HotKeyCommands
{
    public class KaiQianXiangCommand : IHotKeyCommand
    {
        public Action<Window> Handler
        {
            get
            {
                return new Action<Window>((o) =>
                {
                    PrintDocument pdc = new PrintDocument();
                    string printerName = pdc.PrinterSettings.PrinterName;
                    string send = string.Empty;
                    switch (Global.MachineSettings.MachineInformations.QianXiangType)
                    {
                        case 0:
                            send = "" + (char)(27) + (char)(112) + (char)(0) + (char)(60) + (char)(255);//机型一(包括研科T58Z)
                            break;
                        case 1:
                            send = "" + (char)(27) + (char)(64) + (char)(27) + 'J' + (char)(255);    //机型二
                            break;

                    }
                    RawPrinterHelper.SendStringToPrinter(printerName, send);
                });
            }
        }
    }
}
