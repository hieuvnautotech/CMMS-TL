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
    [ApiDescriptionSettings("Standard DB",Tag = "Quản lý Supllier", Order =103)]
    public class SupplierApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;
        public SupplierApiController(IDapperRepository dapperRepository)
        {
            _dapper= dapperRepository;
        }


        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll(string search_name)
        {
            var query = @"select * from dbo.SD_Supplier where 1=1";
            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += " and supplier_name like '%' + @search_name + '%' ";

            }

            var list = _dapper.Context.Query<SupplierModel_GetList>(query, new { search_name });

          
            return Ok(list);

            //var query = @"
            //            select a.*, a.location_id as id , b.type_name
            //            from sd_location a
            //            left join sd_type b on b.type_id=a.type_id 
            //            where 1=1";

            //if (!string.IsNullOrWhiteSpace(search_name))
            //{
            //    query += " and a.location_name like '%' + @search_name + '%'";

            //}


            //var list2 = _dapper.Context.Query<dynamic>(query, new { search_name });
            //return Ok(list.Select(a => new Dictionary<string, object>(a)));

        }

        [HttpPost("add-update-supplier")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] SupplierModel_Input input)
        {

            if (input.supplier_id == null)
            {
                //add
                //input.created_at=DateTime.Now;
                await _dapper.Context.ExecuteAsync(@"
                                                    INSERT INTO [dbo].[SD_Supplier]
                                                               ([supplier_name]                                                             
                                                               ,[created_at])
                                                         VALUES
                                                               (@supplier_name
                                                               ,getdate())
                                                    ", input);
            }
            else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[SD_Supplier]
                                                               set supplier_name=@supplier_name
                                                               
                                                               
                                                                
                                                    where supplier_id=@supplier_id
                                                       
                                                    ", input);
            }

            return Ok();
        }

        [HttpPost("delete-supplier")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] SupplierModel_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[SD_Supplier]
                                                    where supplier_id=@supplier_id
                                                       
                                                    ", input);

            return Ok();
        }


    }
}
