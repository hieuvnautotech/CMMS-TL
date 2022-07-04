using CCMS.Application.Dtos;
using CCMS.Application.Dtos.Breakdown_Maintenance;
using CCMS.Application.Dtos.StandardDB;
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
    [ApiDescriptionSettings("BM",Tag = "BM Working Order", Order = 500)]
    public class WorkingOrderApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;

        public WorkingOrderApiController(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll(string work_order, string equipment_name, string start_date, string end_date)
        {
            var query = @"
                            select 
                              a.*, 
                              b.location_name as location, 
                              c.type_name as type,
                              d.equipment_name,
                              e.status_name,
                              f.priority_name
                            from 
                              Working_Order a 
                              left join SD_Location b on a.location_id = b.location_id 
                              left join SD_Type c on a.type_id = c.type_id 
                              left join SD_Equipment d on a.equipment_id = d.equipment_id
                              left join Status e on a.status_id = e.status_id
                              left join Priority f on a.priority_id = f.priority_id                               
                              where 1=1
                           ";
            if (!string.IsNullOrWhiteSpace(work_order))
            {
                query += " and work_order like '%' + @work_order + '%'";

            }
            if (!string.IsNullOrWhiteSpace(equipment_name))
            {
                query += " and equipment_name like '%' + @equipment_name + '%'";

            }
            if (!string.IsNullOrWhiteSpace(start_date))
            {
                query += " and a.working_date >= @start_date";

            }
            if (!string.IsNullOrWhiteSpace(end_date))
            {
                query += " and a.working_date <= @end_date";

            }
            var list = _dapper.Context.Query<Working_Order_Input>(query, new { work_order, equipment_name, start_date, end_date});
            return Ok(list);
        }

        [HttpPost("add-update")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Working_Order_Input input)
        {

            if (input.working_order_id == null)
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    INSERT INTO [dbo].[Working_Order]
                                                               ([status_id]
                                                               ,[priority_id]
                                                               ,[work_order]
                                                               ,[equipment_id]
                                                               ,[description]
                                                               ,[requestor]
                                                               ,[name]
                                                               ,[due_date]
                                                               ,[working_date]
                                                               ,[location_id]
                                                               ,[file]
                                                               ,[type_id]
                                                               ,[worker]
                                                               ,[remark])
                                                         VALUES
                                                               (@status_id
                                                               ,@priority_id
                                                               ,@work_order
                                                               ,@equipment_id
                                                               ,@description
                                                               ,@requestor
                                                               ,@name
                                                               ,@due_date
                                                               ,getdate()
                                                               ,@location_id
                                                               ,@file
                                                               ,@type_id
                                                               ,@worker
                                                               ,@remark)
                                                    ", input);
            }
            else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[Working_Order]
                                                               set status_id=@status_id
                                                               ,priority_id=@priority_id
                                                               ,work_order=@work_order
                                                               ,equipment_id=@equipment_id
                                                               ,description=@description
                                                               ,requestor=@requestor
                                                               ,name=@name
                                                               ,due_date=@due_date
                                                               ,location_id=@location_id
                                                               ,[file]=@file
                                                               ,type_id=@type_id
                                                               ,worker=@worker
                                                               ,remark=@remark
                                                    where working_order_id=@working_order_id
                                                    ", input);
            }

            return Ok();
        }

        [HttpPost("delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Working_Order_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[Working_Order]
                                                    where working_order_id=@working_order_id
                                                       
                                                    ", input);

            return Ok();
        }
        [HttpGet("get-parts")]
        [AllowAnonymous]
        public IActionResult GetAllParts(int equipment_id)
        {


            var list = _dapper.Context.Query<dynamic>(@"select 
                                                  a.equipment_id, 
                                                  c.*
                                                from 
                                                  Working_Order a 
                                                  left join TT_Equipment_Part b on a.equipment_id = b.equipment_id 
                                                  left join SD_Part c on b.part_id = c.part_id      
                                                  where a.equipment_id = @equipment_id
                                                    ", new { equipment_id });

            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-status")]
        [AllowAnonymous]
        public IActionResult GetStatus()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                status_name as title, 
                                status_id as value
                                from Status");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
        [HttpGet("get-priority")]
        [AllowAnonymous]
        public IActionResult GetPriority()
        {
            var list = _dapper.Context.Query<dynamic>(@"
                                select 
                                priority_name as title, 
                                priority_id as value
                                from Priority");
            return Ok(list.Select(a => new Dictionary<string, object>(a)));
        }
    }
}
