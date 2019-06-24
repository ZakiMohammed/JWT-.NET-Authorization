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
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [Authorize]        
        public IHttpActionResult Get()
        {
            try
            {
                var repo = new ApplicationRepository();
                return Ok(repo.GetUsers());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult Post(User user)
        {
            try
            {
                var repo = new ApplicationRepository();
                if (repo.IsUserExist(user))
                {
                    return Ok(new
                    {
                        Message = Constants.MESSAGE_EXIST
                    });
                }
                else
                {
                    repo.AddUser(user);
                    return Ok(new
                    {
                        Message = Constants.MESSAGE_SUCCESS,
                        User = user
                    });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(UserDTO userDTO)
        {
            try
            {
                var repo = new ApplicationRepository();
                var user = repo.AuthenticateUser(userDTO.UserName, userDTO.Password);                                               
                if (user != null)
                {                    
                    return Ok(TokenManager.GenerateToken(user.UserName));
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
