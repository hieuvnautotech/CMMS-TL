using CCMS.Application.Dtos;
using CCMS.Application.Dtos.StandardDB;
using CCMS.Application.Utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Api
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings("Standard DB",Tag = "Machine", Order =500)]
    public class MachineApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;

        public MachineApiController(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll(string Machine_code)
        {
            var query = @"
                            select 
                              a.*, 
                              b.area_name, 
                              c.type_name, 
                              d.manufacturer_name, 
                              e.supplier_name,
                              f.status_name,
                              g.unit_name,
                              h.location_name
                            from 
                              SD_Machine a 
                              left join SD_Area b on a.area_id = b.area_id 
                              left join SD_Type c on a.type_id = c.type_id 
                              left join SD_Manufacturer d on a.manufacturer_id = d.manufacturer_id 
                              left join SD_Supplier e on a.supplier_id = e.supplier_id     
                              left join Status f on a.status_id = f.status_id   
                              left join SD_Unit g on a.unit_id = g.unit_id
                              left join SD_Location h on a.location_id = h.location_id   
                           ";
            if (!string.IsNullOrWhiteSpace(Machine_code))
            {
                query += " where Machine_code like '%' + @Machine_code + '%'";

            }
            var list = _dapper.Context.Query<Machine_Input>(query, new { Machine_code });
            return Ok(list);
        }

        [HttpPost("add-update")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Machine_Input input)
        {
            input.machine_code = _dapper.Context.QueryFirst<string>("GetNextMachineCode", commandType: CommandType.StoredProcedure);
            if (input.machine_id == null)
            {
                await _dapper.Context.ExecuteAsync (@"
                                                    INSERT INTO [dbo].[SD_Machine]
                                                               ([area_id]
                                                              ,[supplier_id]
                                                              ,[manufacturer_id]
                                                              ,[price]
                                                              ,[serial_number]
                                                              ,[remark]
                                                              ,[made_date]
                                                              ,[install_date]
                                                              ,[use]
                                                              ,[type_id]
                                                              ,[machine_name]
                                                              ,[status_id]
                                                              ,[unit_id]
                                                              ,[location_id]
                                                              ,[machine_code])
                                                         VALUES
                                                               (@area_id
                                                              ,@supplier_id
                                                              ,@manufacturer_id
                                                              ,@price
                                                              ,@serial_number
                                                              ,@remark
                                                              ,getdate()
                                                              ,@install_date
                                                              ,1
                                                              ,@type_id
                                                              ,@machine_name
                                                              ,@status_id
                                                              ,@unit_id
                                                              ,@location_id
                                                              ,@machine_code)
                                                    ", input);
            } else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[SD_Machine]
                                                               set area_id=@area_id
                                                              ,supplier_id=@supplier_id
                                                              ,manufacturer_id=@manufacturer_id
                                                              ,price=@price
                                                              ,serial_number=@serial_number
                                                              ,remark=@remark
                                                              ,install_date=@install_date
                                                              ,type_id=@type_id
                                                              ,machine_name=@machine_name
                                                              ,status_id=@status_id
                                                              ,unit_id=@unit_id
                                                              ,location_id=@location_id
                                                    where machine_id=@machine_id
                                                    ", input);
            }
           
            return Ok();
        }

        [HttpPost("delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Machine_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[SD_Machine]
                                                    where machine_id=@machine_id
                                                       
                                                    ", input);

            return Ok();
        }
        [HttpGet("get-unit")]
        [AllowAnonymous]
        public IActionResult GetUnit()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                unit_name as title, 
                                unit_id as value
                                from SD_Unit");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-area")]
        [AllowAnonymous]
        public IActionResult GetArea()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                area_name as title, 
                                area_id as value
                                from SD_Area");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
    }
}
