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
    [ApiDescriptionSettings("Standard DB",Tag = "StandardDB Staff", Order =300)]
    public class StaffApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;

        public StaffApiController(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll(string search_name)
        {
            var query = @"  select 
                              a.*, 
                              b.Name as department_name
                            from 
                              SD_Staff a 
                              left join Department b on a.department_id = b.DepartmentId 
                        ";
            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += "where staff_name like '%' + @search_name + '%'";

            }
            var list = _dapper.Context.Query<Staff_Input>(query, new { search_name });
            return Ok(list);
        }

        [HttpPost("add-update")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Staff_Input input)
        {

            if (input.staff_id==null)
            {
                //add
                await _dapper.Context.ExecuteAsync (@"
                                                    INSERT INTO [dbo].[SD_Staff]
                                                               ([staff_name]
                                                               ,[department_id]
                                                               ,[call]
                                                               ,[email]
                                                               ,[remark]
                                                               ,[created_at])
                                                         VALUES
                                                               (@staff_name
                                                               ,@department_id
                                                               ,@call
                                                               ,@email
                                                               ,@remark
                                                               ,getdate())
                                                    ", input);
            } else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[SD_Staff]
                                                               set staff_name=@staff_name
                                                               ,department_id=@department_id
                                                               ,call=@call
                                                               ,email=@email
                                                               ,remark=@remark
                                                               ,created_at=getdate()
                                                    where staff_id=@staff_id
                                                    ", input);
            }
           
            return Ok();
        }

        [HttpPost("delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Staff_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[SD_Staff]
                                                    where staff_id=@staff_id
                                                       
                                                    ", input);

            return Ok();
        }
        [HttpGet("get-department")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                DepartmentId as value, 
                                Name as title
                                from Department");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
    }
}
