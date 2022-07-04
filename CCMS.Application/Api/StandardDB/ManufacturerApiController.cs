using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCMS.Application.Dtos;
using CCMS.Application.Dtos.StandardDB;
using CCMS.Application.Enum;
using CCMS.Application.Services;
using CCMS.Core.Filter;
using Dapper;

namespace CCMS.Application.Api.StandardDB
{

    [Route("api/[controller]")]
    [ApiDescriptionSettings("Standard DB",Tag = "Quản lý Manufacturer cua MR Toan", Order =200)]
    public class ManufacturerApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;
        public ManufacturerApiController(IDapperRepository dapperRepository)
        {
            _dapper= dapperRepository;
        }


        [HttpGet("Manufacturer_search")]
        [AllowAnonymous]
        public IActionResult GetAllSearchType(string search_name)
        {
       

            var query = @"
                            select *, manufacturer_id as id
                            from SD_Manufacturer 
                           ";

            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += " where manufacturer_name like '%' + @search_name + '%'";

            }


            var list = _dapper.Context.Query<Manufacturer_Model_GetLit>(query, new { search_name });
            return Ok(list);
        }
        //[HttpPost("Manufacturer_delete")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Delete([FromBody] Manufacturer_Input input)
        //{


        //    await _dapper.Context.ExecuteAsync(@"
        //                                            delete from [dbo].[SD_Manufacturer]
        //                                            where manufacturer_id=@manufacturer_id
        //                                            ", input);

        //    return Ok();
        //}
        //[HttpPost("add-update-Manufacturer")]
        //[AllowAnonymous]
        //public async Task<IActionResult> AddOrUpdate([FromBody] Manufacturer_Input input)
        //{

        //    if (input.manufacturer_id == null)
                
        //    {
        //        if (input.use == null) input.use = true;

                //xét manufacturer_name không trùng manufacturer_name trong database  
                //var isDupName = await _dapper.Context.QueryFirstAsync<bool>(@"
                //                                   select 1 from   [dbo].[SD_Manufacturer] where [manufacturer_name]=@manufacturer_name
                //                                              ", input);

        //        if (isDupName)
        //        {
        //            throw Oops.Oh(ErrorCode.T001);
        //        }
        //        //add

        //        await _dapper.Context.ExecuteAsync(@"
        //                                            INSERT INTO [dbo].[SD_Manufacturer]
        //                                                       ( [manufacturer_name] ,[use])
        //                                                 VALUES
        //                                                       ( @manufacturer_name ,@use )", input);
        //    }
        //    else
        //    {
        //        await _dapper.Context.ExecuteAsync(@"
        //                                            update [dbo].[SD_Manufacturer]
        //                                                       set
        //                                                       manufacturer_name=@manufacturer_name
        //                                                       ,[use]=@use                                                               
        //                                            where manufacturer_id=@manufacturer_id

        //                                            ", input);
        //    }

        //    return Ok();
        //}
        [HttpGet("get-select-data-active")]
        [AllowAnonymous]
        public IActionResult GetDataActive()
        {

            return Ok(new List<SelectBox_Model>
            {
                new SelectBox_Model{value=true,title="Use"},
                 new SelectBox_Model{value=false,title="Dont use"}


            });
        }



    }
}
