using ApiHelper.Models;
using Model_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiHelper.Controllers
{
    [Route("UserName")]
    public class UserNameController : ApiController
    {
        [HttpGet]
        [Route("UserName/GetUserName")]
        [EnableCors(origins: "https://localhost:44315/"  , headers: "*", methods: "*")]
        public string GetUserName()
        {
            //if (local_cookie._user_name == null)
            //{
            //    local_cookie._user_name = Static_Cookie.user_name ;
            //}
            return local_cookie._user_name;
        }
    }
}
