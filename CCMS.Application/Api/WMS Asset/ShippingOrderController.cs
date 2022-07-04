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
    [ApiDescriptionSettings("WMS Asset", Tag = "WMS Shipping Order", Order =400)]
    public class ShippingOrderController : ControllerBase
    {
        private readonly IDapperRepository _dapper;

        public ShippingOrderController(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        [HttpGet("master")]
        [AllowAnonymous]
        public IActionResult GetAll(string so)
        {
            var query = @"
                            select a.*, b.location_name
                            from Shipping_Master a left join SD_Location b on a.location_id = b.location_id
                           ";
            if (!string.IsNullOrWhiteSpace(so))
            {
                query += " where so like '%' + @so + '%'";

            }
            var list = _dapper.Context.Query<Shipping_Master>(query, new { so });
            return Ok(list);
        }
        [HttpPost("add-update-master")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Shipping_Master input)
        {

            if (input.shipping_id==null)
            {
                //add
                await _dapper.Context.ExecuteAsync (@"
                                                    INSERT INTO [dbo].[Shipping_Master]
                                                               ([shipping_name]
                                                               ,[so]
                                                               ,[etd]
                                                               ,[status]
                                                               ,[location_id]
                                                               ,[created_by]
                                                               ,[reg_date]
                                                               ,[remark])
                                                         VALUES
                                                               (@shipping_name
                                                               ,@so
                                                               ,@etd
                                                               ,@status
                                                               ,@location_id
                                                               ,@created_by
                                                               ,@reg_date
                                                               ,@remark)
                                                    ", input);
            } else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[Shipping_Master]
                                                               set shipping_name=@shipping_name
                                                               ,so=@so
                                                               ,etd=@etd
                                                               ,status=@status
                                                               ,location_id=@location_id
                                                               ,created_by=@created_by
                                                               ,reg_date=@reg_date
                                                               ,remark=@remark
                                                    where shipping_id=@shipping_id
                                                    ", input);
            }
           
            return Ok();
        }

        [HttpPost("delete-master")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Shipping_Master input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[Shipping_Master]
                                                    where shipping_id=@shipping_id
                                                       
                                                    ", input);

            return Ok();
        }

        [HttpGet("detail")]
        [AllowAnonymous]
        public IActionResult GetAllDetail(string equipment, string name)
        {
            var query = @" select a.*, b.equipment_code, d.location_name, c.unit_name,e.shipping_name,f.part_name
                            from Shipping_Order_Detail a 
							left join SD_Equipment b on a.equipment_id = b.equipment_id
							left join SD_Unit c on a.unit_id = c.unit_id 
							left join SD_Location d on d.location_id = a.location_id
							left join Shipping_Master e on a.shipping_id=e.shipping_id
							left join SD_Part f on a.part_id=f.part_id
                            where 1=1
                           ";
            if (!string.IsNullOrWhiteSpace(equipment))
            {
                query += " and equipment like '%' + @equipment + '%'";
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query += " and name like '%' + @name + '%'";
            }
            query += " order by a.shipping_id desc";
            var list = _dapper.Context.Query<Shipping_Detail>(query, new { equipment, name });
            return Ok(list);
        }
        [HttpPost("add-update-detail")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdateDetail([FromBody] Shipping_Detail input)
        {

            if (input.shipping_order_id == null)
            {
                //add
                input.created_at = DateTime.Now;
                await _dapper.Context.ExecuteAsync(@"
                                                    INSERT INTO [dbo].[Shipping_Order_Detail]
                                                               ([name]
                                                               ,[shipping_id]
                                                               ,[equipment_id]
                                                               ,[part_id]
                                                               ,[location_id]
                                                               ,[status]
                                                               ,[so_qty]
                                                               ,[unit_id]
                                                               ,[shipped_qty]                                                              
                                                               ,[created_at])
                                                         VALUES
                                                               (@name
                                                               ,@shipping_id
                                                               ,@equipment_id
                                                               ,@part_id
                                                               ,@location_id
                                                               ,@status
                                                               ,@so_qty
                                                               ,@unit_id
                                                               ,@shipped_qty
                                                               ,@created_at)
                                                    ", input);
            }
            else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[Shipping_Order_Detail]
                                                               set name=@name
                                                               ,shipping_id=@shipping_id
                                                               ,equipment_id=@equipment_id
                                                               ,part_id=@part_id
                                                               ,location_id=@location_id
                                                               ,status=@status
                                                               ,so_qty=@so_qty
                                                               ,unit_id=@unit_id
                                                               ,shipped_qty=@shipped_qty
                                                               ,created_by=@created_by
                                                    where shipping_order_id=@shipping_order_id
                                                    ", input);
            }

            return Ok();
        }
        [HttpPost("delete-detail")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteDetail([FromBody] Shipping_Detail input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[Shipping_Order_Detail]
                                                    where shipping_order_id=@shipping_order_id
                                                       
                                                    ", input);

            return Ok();
        }
        [HttpGet("get-location")]
        [AllowAnonymous]
        public IActionResult GetAllLocation()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                location_name as title, 
                                location_id as value
                                from SD_Location");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-equipment")]
        [AllowAnonymous]
        public IActionResult GetAllEquipment()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                equipment_name as title, 
                                equipment_id as value
                                from SD_Equipment");
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
                                from SD_Part
                                where [use]=1");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-shipping")]
        [AllowAnonymous]
        public IActionResult GetAllShipping()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                shipping_name as title, 
                                shipping_id as value
                                from Shipping_Master");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-unit")]
        [AllowAnonymous]
        public IActionResult GetAllUnit()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                unit_name as title, 
                                unit_id as value
                                from SD_Unit");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
    }
}
