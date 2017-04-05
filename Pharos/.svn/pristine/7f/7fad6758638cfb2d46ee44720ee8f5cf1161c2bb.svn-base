using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharos.Store.Retailing.Models
{
    public class UserLogin
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名", Description = "4-20个字符。")]
        [Required(ErrorMessage = "×")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "×")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码", Description = "6-20个字符。")]
        [Required(ErrorMessage = "×")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "×")]
        [DataType(DataType.Password)]
        public string UserPwd { get; set; }

        public bool RememberMe { get; set; }

        public string StoreId { get; set; }

        public int CID { get; set; }

        public string logo { get; set; }

        public int isReadOnly { get; set; }
    }
}