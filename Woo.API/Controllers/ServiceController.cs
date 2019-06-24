using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Woo.Repository;

namespace Woo.API.Controllers
{
    [Authorize]
    public class ServiceController : ApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                var repo = new ApplicationRepository();
                return Ok(repo.GetServices());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Get(int id)
        {
            try
            {
                var repo = new ApplicationRepository();
                return Ok(repo.GetService(id));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
