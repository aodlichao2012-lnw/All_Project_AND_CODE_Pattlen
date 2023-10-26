using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace API_helper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNameController : ControllerBase
    {
        [HttpGet]
        public string GetUserName()
        {
            return Static_Cookie.user_name;
        }
    }
}
