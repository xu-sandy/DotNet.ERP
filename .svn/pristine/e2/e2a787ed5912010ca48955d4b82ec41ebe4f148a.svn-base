using System.IO.Ports;

namespace Pharos.POS.Retailing.Devices.QuickConnectTools
{
    public class SerialPortRequest
    {
        public SerialPortRequest()
        {
            ComPort = "COM1";
            BaudRate = 57600;
            Parity = Parity.None;
            DataBits = 8;
            StopBits = StopBits.One;
            Handshake = Handshake.None;
        }
        public string ComPort { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public Handshake Handshake { get; set; }
    }
}
