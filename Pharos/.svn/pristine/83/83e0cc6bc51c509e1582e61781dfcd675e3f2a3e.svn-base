using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.WeighDevice.ScaleEntity
{
    /// <summary>
    /// 顶尖plu实体
    /// </summary>
    public struct TPlu
    {
        public string Name;//品名 36个字符
        public int LFCode;//生鲜码，1-999999，唯一识别每一种生鲜商品
        public string Code;//货号，10位数字，用来组成条码
        public int BarCode;//条码类型 0-99
        public int UnitPrice;//单价，无小数点模式0-9999999
        public int WeightUnit;//称重单位 0-12
        public string Deptment;//部门，2位数字，用来组成条码
        public double Tare;//皮重，15kg以内
        public int ShlefTime;//保存期0-365
        public int PackageType;//包装类型,0:普通/限重模式,1:定重包装,2:定价包装,3:条码打印机模式
        public double PackageWeight;//包装重量/限重重量,逻辑换算后应在15Kg内
        public int Tolerance;//包装误差,0-20
        public byte Message1;//信息1,0-197
        public byte Reserved;//保留
        public short Reserved1;//保留
        public byte Message2;//保留
        public byte Reserved2;//保留
        public byte MultiLabel;//多标签,0-255,8个Bit位分别对应A0-D1
        public short Rebate;//折扣,0-99
        public int Account;//入账,保留
    }
}
