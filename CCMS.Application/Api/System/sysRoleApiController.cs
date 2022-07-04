using CCMS.Application.Dtos;
using CCMS.Application.Services;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCMS.Application.Dtos.System;

namespace CCMS.Application.Api
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings("System",Tag = "System Role")]
    public class sysRoleaApiController : ControllerBase
    {
        private readonly ISysRoleService _sysRoleService;
        public sysRoleaApiController(ISysRoleService sysRoleService)
        {
            _sysRoleService = sysRoleService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string search_name)
        {
            var list = await _sysRoleService.GetAll(search_name);
            return Ok(list);
        }

        [HttpPost("add-update")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] sysRoleModel input)
        {
            if (input.id == null) {
                await _sysRoleService.Create(input);
            } 
            else{
                await _sysRoleService.Update(input);
            }
           
            return Ok();
        }

        [HttpPost("delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] sysRoleModel input)
        {
            int RoleId = (int)input.id;
            if (RoleId != 0)
            {
                await _sysRoleService.Delete(RoleId);
            }

            return Ok();
        }
    }
}
