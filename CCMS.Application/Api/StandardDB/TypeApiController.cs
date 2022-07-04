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
    [ApiDescriptionSettings("Standard DB",Tag = "Quản lý Type cua MR Toan",Order =102)]
    public class TypeApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;
        public TypeApiController(IDapperRepository dapperRepository)
        {
            _dapper= dapperRepository;
        }


        [HttpGet("type_search")]
        [AllowAnonymous]
        public IActionResult GetAllSearchType(string search_name)
        {
            var query = @"
                            select *, type_id as id
                            from sd_type 
                           ";

            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += " where type_name like '%' + @search_name + '%'";

            }


            var list = _dapper.Context.Query<Type_Model_GetLit>(query, new { search_name });
            return Ok(list);
        }

     

        [HttpPost("add-update-type")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Type_Input input)
        {


            if (input.type_id == null)
            {
                if (input.use == null) input.use = true;
                //add
                await _dapper.Context.ExecuteAsync(@"
                                                    INSERT INTO [dbo].[SD_Type]
                                                               ( [type_name] ,[use]     )
                                                         VALUES
                                                               ( @type_name ,@use )", input);
            }
            else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[SD_Type]
                                                               set
                                                               type_name=@type_name
                                                               ,[use]=@use                                                               
                                                    where type_id=@type_id

                                                    ", input);
            }

            return Ok();
        }

        [HttpPost("delete-type")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Type_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[SD_type]
                                                    where type_id=@type_id
                                                    ", input);

            return Ok();
        }


        [HttpGet("get-select-data-active")]
        [AllowAnonymous]
        public IActionResult GetDataActive()
        {
           
            return Ok(new List<SelectBox_Model>
            {
                new SelectBox_Model{value=true,title="Use"},
                 new SelectBox_Model{value=false,title="Not Use" }
            });
        }


    }
}
