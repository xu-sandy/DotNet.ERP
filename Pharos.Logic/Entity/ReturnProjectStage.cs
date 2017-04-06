using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Pharos.Logic.Entity
{
    //ReturnProjectStage
    public class ReturnProjectStage
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// guid 用于关联
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 返还类型
        /// </summary>
        public int ReturnType { get; set; }
        /// <summary>
        /// 多少天后返还
        /// </summary>
        public int AfterDays { get; set; }
        /// <summary>
        /// 赠送时间
        /// </summary>
        public string ReturnDT { get; set; }
        /// <summary>
        /// 赠送值
        /// </summary>
        public decimal ReturnVal { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUID { get; set; }

    }
}