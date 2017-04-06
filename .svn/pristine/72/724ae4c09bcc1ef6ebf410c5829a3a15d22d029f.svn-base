using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Pharos.POS.Retailing.Printers
{
    public class PrinterService
    {

        public void DoPrint<T>(T model, PrintTemplate tpl)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var doc = PrintLayoutHelper.GetDocument();
                doc.Format<T>(tpl, model);

                var q = GetPrinter("Microsoft XPS Document Writer");
                var pd = new PrintDialog { PrintQueue = q };
                if (q != null || pd.ShowDialog().GetValueOrDefault(false))
                {
                    //doc.Background = Brushes.Transparent;
                    doc.PageHeight = pd.PrintableAreaHeight;
                    doc.PageWidth = pd.PrintableAreaWidth;
                    doc.PagePadding = new Thickness(25);
                   // doc.ColumnGap = 0;
                    //doc.ColumnWidth = (doc.PageWidth -
                    //                       doc.ColumnGap -
                    //                       doc.PagePadding.Left -
                    //                       doc.PagePadding.Right);
                    pd.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "");
                }
            }));

        }

        public PrintQueue GetPrinter(string shareName)
        {
            var result = FindPrinterByName(shareName);
            if (result == null) result = FindPrinterByShareName(shareName);
            return result;
        }
        internal PrintQueue FindPrinterByName(string printerName)
        {
            return Printers.FirstOrDefault(x => x.FullName == printerName);
        }
        public PrintQueue FindPrinterByShareName(string shareName)
        {
            return Printers.FirstOrDefault(x => x.HostingPrintServer.Name + "\\" + x.ShareName == shareName);
        }
        private PrintQueueCollection _printers;
        internal PrintQueueCollection Printers
        {
            get
            {
                return _printers ?? (_printers = PrintServer.GetPrintQueues(new[]
                                                                            {
                                                                                EnumeratedPrintQueueTypes.Local,
                                                                                EnumeratedPrintQueueTypes.Connections
                                                                            }));
            }
        }
        private static LocalPrintServer _printServer;
        internal static LocalPrintServer PrintServer
        {
            get { return _printServer ?? (_printServer = new LocalPrintServer()); }
        }
    }
}
