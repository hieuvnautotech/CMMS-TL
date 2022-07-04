using CCMS.Application.Dtos;
using CCMS.Application.Dtos.StandardDB;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Api
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings("Standard DB",Tag = "StandardDB Unit", Order =101)]
    public class UnitApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;

        public UnitApiController(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllUnit(string search_name)
        {
            var query = @"
                            select a.*, b.manufacturer_name
                            from sd_unit a left join sd_manufacturer b on a.manufacturer_id = b.manufacturer_id
                           ";
            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += " where unit_name like '%' + @search_name + '%'";

            }
            var list = _dapper.Context.Query<Unit_Input>(query, new { search_name });
            return Ok(list);
        }

        [HttpPost("add-update-unit")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Unit_Input input)
        {

            if (input.unit_id==null)
            {
                //add
                await _dapper.Context.ExecuteAsync (@"
                                                    INSERT INTO [dbo].[SD_Unit]
                                                               ([unit_name]
                                                               ,[use]
                                                               ,[unit_remark]
                                                               ,[manufacturer_id])
                                                         VALUES
                                                               (@unit_name
                                                               ,1
                                                               ,@unit_remark
                                                               ,@manufacturer_id)
                                                    ", input);
            } else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[SD_Unit]
                                                               set unit_name=@unit_name
                                                               ,unit_remark=@unit_remark
                                                               ,manufacturer_id=@manufacturer_id
                                                    where unit_id=@unit_id
                                                    ", input);
            }
           
            return Ok();
        }

        [HttpPost("delete-unit")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Unit_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[SD_Unit]
                                                    where unit_id=@unit_id      
                                                    ", input);

            return Ok();
        }
    }
}
