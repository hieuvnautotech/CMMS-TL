using System;
using System.Collections.Generic;
using System.Data;
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
    [ApiDescriptionSettings("Standard DB",Tag = "Quản lý Part cua MR Toan", Order = 201)]
    public class PartApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;
        private readonly IPartService _PartService;
        public PartApiController(IPartService PartService, IDapperRepository dapperRepository)
        {
            _dapper= dapperRepository;
            _PartService = PartService;
        }


        [HttpGet("Part_search")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSearchPart(string search_name)
        {
       

            var query = @"select a.part_id, a.part_id as id, a.part_code,a.part_name,a.spec,a.price,a.created_at,a.unit,a.type_id,a.location_id,
                            a.manufacturer_id,a.remark,a.[use],b.location_name,c.manufacturer_name,d.type_name
                            from SD_Part a
                            left join SD_Location b on a.location_id=b.location_id
                            left join SD_Manufacturer c on a.manufacturer_id=c.manufacturer_id
                            left join SD_Type d on a.type_id=d.type_id
                            where 1=1 and a.[use]=1";
            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += "and part_name like '%' + @search_name + '%'";

            }
            var list = _dapper.Context.Query<Part_Model_GetList>(query, new { search_name });
            return Ok(list);
        }
        [HttpGet("get-all-type")]
        [AllowAnonymous]
        public IActionResult GetAllType()
        {

            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                type_name as title, 
                                type_id as value
                                from sd_type 
                                where [use]=1");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-all-location")]
        [AllowAnonymous]
        public IActionResult GetAllLocation()
        {

            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                location_name as title, 
                                location_id as value
                                from sd_location");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-all-Manufacturer")]
        [AllowAnonymous]
        public IActionResult GetAllManufacturer()
        {

            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                manufacturer_name as title, 
                                manufacturer_id as value
                                from sd_Manufacturer 
                                where [use]=1");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }

        [HttpPost("Part_delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Part_Input input)

        {
            int id = await _PartService.DeletePartInfo(input.part_id);
            return Ok();
        }
        [HttpPost("add-update-Part")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Part_Input input)
        {

            if (input.part_id == null)

            {
                input.created_at = DateTime.Now;
                var part_code = _dapper.Context.QueryFirst<string>("GetNextPartCode", commandType: CommandType.StoredProcedure);
                input.part_code= part_code;
                //  var param = new DynamicParameters();
                //  param.Add("@somethingId", dbType: DbType.Int32, value: somethingId, direction: ParameterDirection.Input);

                //var result = _dapper.Context.Execute<Something>(SomethingEnum.spMyStoredProcedure, param);

                //var code = Db.Ado.UseStoredProcedure().SqlQuerySingle<ExpandoObject>("GetNextPartCode");

                //var procedure = "[Sales by Year]";
                //var values = new { Beginning_Date = "2017.1.1", Ending_Date = "2017.12.31" };
                //var results = connection.Query(procedure, values, commandType: CommandType.StoredProcedure).ToList();

                if (input.use == null) input.use = true;
                //xét manufacturer_name không trùng manufacturer_name trong database
                var isDupName = await _dapper.Context.ExecuteScalarAsync<bool>(@" select 1 from   [dbo].[SD_Part] where part_name=@part_name ", input);

                if (isDupName)
                {
                    throw Oops.Oh(ErrorCode.T001);
                }
                int id = await _PartService.insertPartInfo(input);

            }
            else
            {
                int id = await _PartService.UpdatePartInfo(input);
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
