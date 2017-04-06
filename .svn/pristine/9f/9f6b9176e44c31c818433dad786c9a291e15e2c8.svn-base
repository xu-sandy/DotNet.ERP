using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility;

namespace Pharos.Logic.WeighDevice
{
    interface IWeighScale<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 数据传送
        /// </summary>
        /// <param name="entitys">数据集合</param>
        /// <param name="ip">电子秤ip</param>
        /// <param name="isClear">是否清空原有数据</param>
        /// <returns></returns>
        OpResult TransferData(List<TEntity> entitys, List<string> ips, bool isClear);
    }
}
