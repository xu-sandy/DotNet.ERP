using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.Sys.Models
{
    public class QueryUserModel
    {

        public string Keyword { get; set; }
        public int OrganizationId { get; set; }
        public int DepartmentId { get; set; }
        public string RoleGroupsId { get; set; }
        public short? Status { get; set; }
    }
}
