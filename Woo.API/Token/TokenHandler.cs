using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Woo.API
{
    internal class TokenHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var statusCode = HttpStatusCode.Unauthorized;
            var token = string.Empty;
            
            if (!TokenManager.TryRetrieveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;                
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                var principal = TokenManager.GetPrincipal(token);
                if (principal != null)
                {
                    Thread.CurrentPrincipal = principal;
                    HttpContext.Current.User = principal;

                    return base.SendAsync(request, cancellationToken);
                }
                else
                {
                    statusCode = HttpStatusCode.Unauthorized;
                }
            }
            catch (SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
        }        
    }
}