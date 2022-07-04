using CCMS.Application.Dtos;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Api
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings("Standard DB",Tag = "StandardDB Area", Order =101)]
    public class AreaApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;

        public AreaApiController(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllArea(string search_name)
        {
            var query = @"select * from sd_area";
            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += " where area_name like '%' + @search_name + '%'";
            }
            var list = _dapper.Context.Query<Area_Input>(query, new { search_name });
            return Ok(list);
        }

        [HttpPost("add-update-area")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Area_Input input)
        {

           
            if (input.area_id==null)
            {
                //add
                await _dapper.Context.ExecuteAsync (@"
                                                    INSERT INTO [dbo].[SD_Area]
                                                               ([area_name]
                                                               ,[type_id]
                                                               ,[remark])
                                                         VALUES
                                                               (@area_name
                                                               ,null
                                                               ,@remark)
                                                    ", input);
            } else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[SD_Area]
                                                               set area_name=@area_name
                                                               ,remark=@remark
                                                    where area_id=@area_id
                                                    ", input);
            }
           
            return Ok();
        }

        [HttpPost("delete-area")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Area_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[SD_Area]
                                                    where area_id=@area_id
                                                       
                                                    ", input);

            return Ok();
        }
    }
}
