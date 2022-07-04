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
    [ApiDescriptionSettings("Standard DB",Tag = "Equipment", Order =500)]
    public class EquipmentApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;

        public EquipmentApiController(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll(string equipment_code)
        {
            var query = @"
                            select 
                              a.*, 
                              b.location_name as location, 
                              c.type_name as type, 
                              d.manufacturer_name as manufacturer, 
                              e.supplier_name as supplier 
                            from 
                              SD_Equipment a 
                              left join SD_Location b on a.location_id = b.location_id 
                              left join SD_Type c on a.type_id = c.type_id 
                              left join SD_Manufacturer d on a.manufacturer_id = d.manufacturer_id 
                              left join SD_Supplier e on a.supplier_id = e.supplier_id                     
                           ";
            if (!string.IsNullOrWhiteSpace(equipment_code))
            {
                query += " where equipment_code like '%' + @equipment_code + '%'";

            }
            var list = _dapper.Context.Query<Equipment_Input>(query, new { equipment_code });
            return Ok(list);
        }

        [HttpPost("add-update")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Equipment_Input input)
        {

            if (input.equipment_id==null)
            {
                //add
                input.equipment_code = _dapper.Context.QueryFirst<string>("GetNextEquipmentCode", commandType: CommandType.StoredProcedure);
                await _dapper.Context.ExecuteAsync (@"
                                                    INSERT INTO [dbo].[SD_Equipment]
                                                               ([equipment_code]
                                                               ,[equipment_name]
                                                               ,[status]
                                                               ,[equipment_value]
                                                               ,[serial_number]
                                                               ,[price]
                                                               ,[made_date]
                                                               ,[install_date]
                                                               ,[created_at]
                                                               ,[use]
                                                               ,[supplier_id]
                                                               ,[type_id]
                                                               ,[location_id]
                                                               ,[manufacturer_id]
                                                               ,[remark]
                                                               ,[created_by]
                                                               ,[level]
                                                               ,[image_url])
                                                         VALUES
                                                               (@equipment_code
                                                               ,@equipment_name
                                                               ,'INSTOCK'
                                                               ,@equipment_value
                                                               ,@serial_number
                                                               ,@price
                                                               ,@made_date
                                                               ,@install_date
                                                               ,getdate()
                                                               ,1
                                                               ,@supplier_id
                                                               ,@type_id
                                                               ,@location_id
                                                               ,@manufacturer_id
                                                               ,@remark
                                                               ,@created_by
                                                               ,@level
                                                               ,@image_url)
                                                    ", input);
            } else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[SD_Equipment]
                                                               set equipment_code=@equipment_code
                                                               ,equipment_name=@equipment_name
                                                               ,status=@status
                                                               ,equipment_value=@equipment_value
                                                               ,serial_number=@serial_number
                                                               ,price=@price
                                                               ,made_date=@made_date
                                                               ,install_date=@install_date
                                                               ,[use]=@use
                                                               ,supplier_id=@supplier_id
                                                               ,type_id=@type_id
                                                               ,location_id=@location_id
                                                               ,manufacturer_id=@manufacturer_id
                                                               ,remark=@remark
                                                               ,created_by=@created_by
                                                               ,[level]=@level
                                                               ,image_url=@image_url
                                                    where equipment_id=@equipment_id
                                                    ", input);
            }
           
            return Ok();
        }

        [HttpPost("delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Equipment_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[SD_Equipment]
                                                    where equipment_id=@equipment_id
                                                       
                                                    ", input);

            return Ok();
        }
        [HttpGet("get-manufacturer")]
        [AllowAnonymous]
        public IActionResult GetAllManufacturer()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                manufacturer_name as title, 
                                manufacturer_id as value
                                from SD_Manufacturer");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-supplier")]
        [AllowAnonymous]
        public IActionResult GetAllSupplier()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                supplier_name as title, 
                                supplier_id as value
                                from SD_Supplier");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-type")]
        [AllowAnonymous]
        public IActionResult GetAllType()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                type_name as title, 
                                type_id as value
                                from SD_Type");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-part")]
        [AllowAnonymous]
        public IActionResult GetAllPart()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                part_name as title, 
                                part_id as value
                                from SD_Part");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-part-collection")]
        [AllowAnonymous]
        public IActionResult GetAllPartCollection()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                                select 
                                                  a.equipment_id, 
                                                  c.part_name
                                                from 
                                                  SD_Equipment a 
                                                  left join TT_Equipment_Part b on a.equipment_id = b.equipment_id 
                                                  left join SD_Part c on b.part_id = c.part_id      
                                                ");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpPost("get-part-id")]
        [AllowAnonymous]
        public IActionResult GetAllPartId([FromBody] Equipment_Input input)
        {
            var id = input.equipment_id;
      
            var list = _dapper.Context.Query<dynamic>(@"
                                                select 
                                                  a.part_id as value, 
                                                  b.part_name as title
                                                from 
                                                  TT_Equipment_Part a 
                                                  left join SD_Part b on a.part_id = b.part_id 
                                                  where a.equipment_id = @id 
                                                ", new { id });
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpPost("change-part")]
        [AllowAnonymous]
        public async Task<IActionResult> Change([FromBody] Equipment_Input input)
        {
            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[TT_Equipment_Part]
                                                    where equipment_id=@equipment_id                                                
                                                    ", input);
            var equipment_id = input.equipment_id;

            if (equipment_id == null)
            {
                var equipment_code= input.equipment_code;
                var query = @"
                            select 
                              a.*
                            from 
                              SD_Equipment a 
                            order by equipment_id desc;
                           ";
                var obj = _dapper.Context.QueryFirstOrDefault<Equipment_Input>(query, new { equipment_code });
                equipment_id = obj.equipment_id;
            }
            foreach (var part in input.partCollection)
            {
                var part_id = part.value;
                await _dapper.Context.ExecuteAsync(@"
                                                    INSERT INTO [dbo].[TT_Equipment_Part]
                                                               ([equipment_id]
                                                               ,[part_id])
                                                         VALUES
                                                               (@equipment_id
                                                               ,@part_id)
                                                    ", new { equipment_id, part_id });
            }
            return Ok();
        }
    }
}
