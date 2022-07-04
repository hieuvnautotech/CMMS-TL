using CCMS.Application.Dtos;
using CCMS.Application.Utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Api
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings("Standard DB",Tag = "Location", Order =101)]
    public class LocationApiController: ControllerBase
    {
        private readonly IDapperRepository _dapper;

        public LocationApiController(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllLocations(int? page, int?pagesize, string search_name)
        {
            const string selectQuery = @" ;WITH _data AS (
                      SELECT a.*, a.location_id as id , b.type_name
                            from sd_location a
                            left join sd_type b on b.type_id=a.type_id 
                      /**where**/
                    ),
                      _count AS (
                        SELECT COUNT(1) AS TotalCount FROM _data
                    )
                    SELECT * FROM _data CROSS APPLY _count /**orderby**/ OFFSET @PageIndex * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY";

            SqlBuilder builder = new SqlBuilder();

            var selector = builder.AddTemplate(selectQuery, new { PageIndex = page??0, PageSize = pagesize??1000 });

            if (!string.IsNullOrEmpty(search_name))
            {
                var @searchname = "%" + search_name + "%";
                builder.Where("a.location_name Like @searchname", new { searchname = @searchname });
            }

            builder.OrderBy("location_id desc");

            var rows = _dapper.Context.Query<dynamic>(selector.RawSql, selector.Parameters).ToList(); ;

            return Ok(new PagedResults<dynamic>
            {
                 Items=rows,
                  TotalCount=rows.Count==0?0:  rows[0].TotalCount
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


        [HttpPost("add-update-location")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Location_Input input)
        {

            if (input.location_id==null)
            {
                //add
                await _dapper.Context.ExecuteAsync (@"
                                                    INSERT INTO [dbo].[SD_Location]
                                                               ([location_code]
                                                               ,[location_name]
                                                               ,[type_id]
                                                               ,[remark]
                                                               ,[created_at])
                                                         VALUES
                                                               (@location_code
                                                               ,@location_name
                                                               ,@type_id
                                                               ,@remark
                                                               ,getdate())
                                                    ", input);
            } else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[SD_Location]
                                                               set location_code=@location_code
                                                               ,location_name=@location_name
                                                               ,remark=@remark
                                                                ,type_id=@type_id
                                                    where location_id=@location_id
                                                       
                                                    ", input);
            }
           
            return Ok();
        }

        [HttpPost("delete-location")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Location_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[SD_Location]
                                                    where location_id=@id
                                                    ", input);

            return Ok();
        }
    }
}
