﻿using Pharos.Logic.ApiData.Pos.ValueObject;

namespace Pharos.Api.Retailing.Models.Pos
{
    public class DeviceRequest : BaseApiParams
    {
        /// <summary>
        /// 设备类型
        /// </summary>
        public DeviceType Type { get; set; }

    }
}