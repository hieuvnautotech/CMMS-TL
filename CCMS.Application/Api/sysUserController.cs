using CCMS.Application.Dtos;
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
    [ApiDescriptionSettings(Tag = "Test Api", Order =100)]
    
    public class TestApiController : ControllerBase
    {
        //private readonly IDapperRepository _dapperRepository;

        private readonly IUserService _userSvr;
        public TestApiController(IUserService userSvr, ISysCacheService ss)
        {
            _userSvr = userSvr;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllData(string p1, int p2)
        {
           
            return Ok(new List<string>() { "a","b"});
        }

        [HttpPost("postdata1")]
        [AllowAnonymous]
        public IActionResult PostData([FromBody] testModel model)
        {

            return Ok(model);
        }




    }
}
