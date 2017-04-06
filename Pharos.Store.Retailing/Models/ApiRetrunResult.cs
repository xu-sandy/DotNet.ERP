using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharos.Store.Retailing.Models
{
    public class ApiRetrunResult<T>
        where T : class
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }
    }
}