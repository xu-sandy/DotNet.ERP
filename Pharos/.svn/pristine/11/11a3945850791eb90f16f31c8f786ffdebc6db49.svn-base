using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Pharos.Logic.ApiData.Pos.ValueObject
{
    public class LoginResult
    {
        /// <summary>
        /// 员工工号
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 商店
        /// </summary>
        public string StoreName { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public string OperateAuth { get; set; }
        /// <summary>
        /// 权限字典
        /// </summary>
        [JsonProperty("OperateAuth")]
        [XmlElement("OperateAuth")]
        public Dictionary<string, List<int>> OperateAuthDict
        {
            get
            {
                Dictionary<string, List<int>> result = new Dictionary<string, List<int>>();
                if (!string.IsNullOrEmpty(OperateAuth))
                {
                    var groups = OperateAuth.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in groups)
                    {
                        var role = item.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (role.Length == 2)
                        {
                            if (result.Keys.Contains(role[0]))
                            {
                                result[role[0]].Add(Convert.ToInt32(role[1]));
                            }
                            else
                            {
                                var list = new List<int>();
                                list.Add(Convert.ToInt32(role[1]));
                                result.Add(role[0], list);
                            }
                        }
                    }
                }
                return result;
            }
        }
    }
}
