using CCMS.Application.Services;
using CCMS.Core.Filter;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Api
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings(Tag = "Quản lý user")]
    
    public class sysUserController: ControllerBase
    {
        //private readonly IDapperRepository _dapperRepository;

        private readonly IUserService _userSvr;
        public sysUserController(IUserService userSvr, ISysCacheService ss)
        {
            _userSvr = userSvr;
        }

        [HttpGet]
       [AuthorizeRole("admin","role2")]
        public IActionResult GetUsers()
        {
          
            var data = _userSvr.GetUsers();
            return Ok(data);
        }

   



    }
}
