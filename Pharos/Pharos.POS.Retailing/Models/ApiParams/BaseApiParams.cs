
namespace Pharos.POS.Retailing.Models.ApiParams
{
    public class BaseApiParams
    {
        /// <summary>
        /// 门店id
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// pos编号
        /// </summary>
        public string MachineSn { get; set; }
        /// <summary>
        /// 企业标识
        /// </summary>
        public int CID { get; set; }
        /// <summary>
        /// 机器编码DeviceSn
        /// </summary>
        public string DeviceSn { get { return Global.MachineSettings.MachineInformations.DeviceId; } }
    }
}
