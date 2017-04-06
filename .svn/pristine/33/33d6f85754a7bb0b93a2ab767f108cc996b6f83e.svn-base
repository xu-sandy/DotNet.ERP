using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Pharos.Utility;
using Pharos.Logic;
using Pharos.Logic.BLL;

namespace Pharos.CMS.Retailing.Controllers
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
    }
}