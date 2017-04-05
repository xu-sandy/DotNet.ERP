using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Pharos.Api.Retailing.Models;
namespace Pharos.Api.Retailing.Controllers
{
    /// <summary>
    /// 权限
    /// </summary>
    public class AuthenticateController : ApiController
    {
        [HttpPost]
        public object token()
        {
            if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    return GetAuthToken(basicAuthenticationIdentity.UID);
                }
            }
            return GetAuthToken("");
        }
        private HttpResponseMessage GetAuthToken(string userId)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
            response.Headers.Add("Token",Guid.NewGuid().ToString());
            response.Headers.Add("TokenExpiry", "900");
            response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
            return response;
        }
    }
}