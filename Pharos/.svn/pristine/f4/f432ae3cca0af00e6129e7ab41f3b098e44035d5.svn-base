using Pharos.Barcode.Retailing.DeviceDrivers.ZMWINPrinter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Pharos.Frame.Wpf.ViewModels;
using Pharos.Barcode.Retailing.Models.BarCodePriters.ZMIN;
using Newtonsoft.Json;
using System.IO;

namespace Pharos.Barcode.Retailing.Models
{
    public class BarCodePrinter : BaseViewModel
    {
        static BarCodePrinter()
        {
            Current = new BarCodePrinter();
        }
        public IEnumerable<string> SupportBarCodePrinters
        {
            get
            {
                return new List<string>()
                    {
                        "ZMIN X1"
                    };
            }
        }

        public static BarCodePrinter Current { get; private set; }


        public BarCodePrintSettings CurrentSettings { get; set; }

        public List<ZMINPrinterSettings> PrinterSettingGroups { get; set; }

        private IEnumerable<string> runningBarCodePrinters;
        public IEnumerable<string> RunningBarCodePrinters
        {
            get
            {
                if (runningBarCodePrinters == null)
                {
                    RefreshLocalPrinter(false);
                }
                return runningBarCodePrinters;
            }
        }

        private string currentBarCodePrinter;
        public string CurrentBarCodePrinter
        {
            get { return currentBarCodePrinter; }
            set
            {
                currentBarCodePrinter = value;
                this.OnPropertyChanged(o => o.CurrentBarCodePrinter);
                switch (value)
                {
                    case "ZMIN X1":
                        var str = File.ReadAllText("Config/ZMIN_X1.json");
                        var result = JsonConvert.DeserializeObject<List<ZMINPrinterSettings>>(str);
                        PrinterSettingGroups = result;
                        if (result != null)
                            CurrentSettings = result.FirstOrDefault();
                        break;
                }
            }
        }

        public void RefreshLocalPrinter(bool sendChanged = true)
        {
            var result = LocalPrinter.GetLocalPrinters().Where(o => SupportBarCodePrinters.Contains(o));
            runningBarCodePrinters = result;
            CurrentBarCodePrinter = result.FirstOrDefault();
            if (sendChanged)
                this.OnPropertyChanged(o => o.RunningBarCodePrinters);
        }
        public void PrintBarCode(Dtos.ProductDto product, uint printNum = 1, uint repeatNum = 1)
        {
            switch (CurrentBarCodePrinter)
            {
                case "ZMIN X1":
                    new ZMINPrinter().Print(CurrentBarCodePrinter, product, CurrentSettings, printNum, repeatNum);
                    break;
            }

        }
    }


}
