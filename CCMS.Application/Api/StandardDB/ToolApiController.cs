using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCMS.Application.Dtos;
using CCMS.Application.Dtos.StandardDB;
using CCMS.Application.Services;
using CCMS.Core.Filter;
using Dapper;

namespace CCMS.Application.Api.StandardDB
{

    [Route("api/[controller]")]
    [ApiDescriptionSettings("Standard DB",Tag = "Quản lý Tool")]
    public class ToolApiController : ControllerBase
    {
        private readonly IToolService _ToolService;
        private readonly IDapperRepository _dapper;
        public ToolApiController(IToolService ToolService, IDapperRepository dapperRepository)
        {
            _dapper= dapperRepository;
            _ToolService = ToolService;
        }


        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get-All-Tool")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string search_name)
        {
            var list = await _ToolService.GetAll(search_name);
            return Ok(list);
        }

        [HttpPost("add-update-tool")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] ToolModel_GetList input)
        {
            try
            {
                if (input.tool_id >0)
                {
                    int id = await _ToolService.insertToolInfo(input);
                }
                else
                {
                    int id = await _ToolService.UpdateToolInfo(input);
                }
                return Ok();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost("delete-tool")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] ToolModel_GetList input)
        {
            int id = await _ToolService.DeleteToolInfo(input.tool_id);
            return Ok();
        }
        [HttpGet("get-all-manufacturer_id")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllManufacturerId()
        {
            var list = await _ToolService.GetAllManufacturerId();
            return Ok(list.Select(a => new Dictionary<string, object>(a)));

        }
        [HttpGet("get-all-type_id")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTypeId()
        {
            var list = await _ToolService.GetAllTypeId();

            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
    }
}
