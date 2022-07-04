using CCMS.Application.Dtos;
using CCMS.Application.Services;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCMS.Application.Dtos.StandardDB;

namespace CCMS.Application.Api
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings("Standard DB",Tag = "User Account")]
    public class UserAccountApiController : ControllerBase
    {
        private readonly IUserAccountService _UserAccountService;
        public UserAccountApiController(IUserAccountService UserAccountService)
        {
            _UserAccountService = UserAccountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string search_name)
        {
            var list = await _UserAccountService.GetAll(search_name);
            return Ok(list);
        }

        [HttpPost("add-update")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] UserAccountModel input)
        {
            if (input.userid == null) {
                await _UserAccountService.Create(input);
            } 
            else{
                await _UserAccountService.Update(input);
            }
           
            return Ok();
        }

        [HttpPost("delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] UserAccountModel input)
        {
            int UserId = (int) input.userid;
            if (UserId != 0)
            {
                await _UserAccountService.Delete(UserId);
            }

            return Ok();
        }

        [HttpGet("get-staff")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStaff()
        {
            var list = await _UserAccountService.GetSelectStaff();
            return Ok(list);
        }
    }
}
