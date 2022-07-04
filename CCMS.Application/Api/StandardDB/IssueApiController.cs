using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCMS.Application.Dtos;
using CCMS.Application.Dtos.StandardDB;
using CCMS.Application.Enum;
using CCMS.Application.Services;
using CCMS.Application.Utils;
using CCMS.Core.Filter;
using Dapper;

namespace CCMS.Application.Api.StandardDB
{

    [Route("api/[controller]")]
    [ApiDescriptionSettings("Standard DB",Tag = "Quản lý Issue cua MR Toan", Order =201)]
    public class IssueApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;
        public IssueApiController(IDapperRepository dapperRepository)
        {
            _dapper= dapperRepository;
        }


        [HttpGet("Issue_search")]
        [AllowAnonymous]
        public IActionResult GetAllSearchType(int? page, int? pagesize, string search_name)
        {
            const string selectQuery = @" ;WITH _data AS (
                      select a.*, a.issue_id as id, b.type_name,FORMAT( a.created_at, 'yyyy-MM-dd HH:mm:ss' ) AS created_at_format
                          from SD_Issue a 
                          left join SD_Type b on b.type_id=a.type_id
                           where 1=1
                    ),
                      _count AS (
                        SELECT COUNT(1) AS TotalCount FROM _data
                    )
                    SELECT * FROM _data CROSS APPLY _count /**orderby**/ OFFSET @PageIndex * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";

            SqlBuilder builder = new SqlBuilder();
            var selector = builder.AddTemplate(selectQuery, new { PageIndex = page ?? 0, PageSize = pagesize ?? 1000 });
                if (!string.IsNullOrEmpty(search_name))
                {
                builder.Where("a.issue_name = @search_name ", new { search_name });
                }

            

                builder.OrderBy("id desc");

            var rows = _dapper.Context.Query<dynamic>(selector.RawSql, selector.Parameters).ToList(); ;

            return Ok(new PagedResults<dynamic>
            {
                Items = rows,
                TotalCount = rows.Count == 0 ? 0 : rows[0].TotalCount
            });

        }
        [HttpGet("get-all-types")]
        [AllowAnonymous]
        public IActionResult GetAllTypes()
        {

            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                type_name as title, 
                                type_id as value
                                from sd_type a
                                where [use]=1");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpPost("Issue_delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Issue_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[SD_Issue]
                                                    where issue_id=@issue_id
                                                    ", input);

            return Ok();
        }
        [HttpPost("add-update-Issue")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Issue_Input input)
        {

            if (input.issue_id == null)
                
            {
                input.created_at=DateTime.Now;
                if (input.use == null) input.use = true;

                //xét manufacturer_name không trùng manufacturer_name trong database  
                var isDupName = await _dapper.Context.ExecuteScalarAsync<bool>(@"
                                                   select 1 from   [dbo].[SD_Issue] where issue_name=@issue_name
                                                              ", input);

                if (isDupName)
                {
                    throw Oops.Oh(ErrorCode.T001);
                }
                //add

                await _dapper.Context.ExecuteAsync(@"
                                                    INSERT INTO [dbo].[SD_Issue]
                                                               ( [issue_name] ,[description],[type_id],[use],[created_at])
                                                         VALUES
                                                               ( @issue_name,@description,@type_id,@use,@created_at )", input);
            }
            else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[SD_Issue]
                                                               set
                                                               issue_name=@issue_name,
                                                               description=@description,
                                                               type_id=@type_id
                                                               ,[use]=@use                                                               
                                                    where issue_id=@issue_id", input);
            }
            return Ok();
        }
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
