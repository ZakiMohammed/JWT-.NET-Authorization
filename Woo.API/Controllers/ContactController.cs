using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Woo.Model;
using Woo.Repository;

namespace Woo.API.Controllers
{
    [Authorize]
    public class ContactController : ApiController
    {
        public void Post(Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    contact = new Contact();
                }
                else
                {
                    var repo = new ApplicationRepository();
                    repo.AddContact(contact);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
