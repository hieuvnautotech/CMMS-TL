using CCMS.Application.Dtos;
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
    [ApiDescriptionSettings("Standard DB",Tag = "StandardDB Bom", Order =6969)]
    public class BomApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;

        public BomApiController(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll(string bom_code)
        {
            var query = @"
                            select 
                              a.*, 
                              b.location_name as location, 
                              c.type_name as type, 
                              d.manufacturer_name as manufacturer, 
                              e.supplier_name as supplier,
                              f.created_at as bom_created_at, f.created_by as bom_created_by, f.*
                            from 
                              SD_Bom f
                              left join SD_Equipment a on f.equipment_id = a.equipment_id
                              left join SD_Location b on a.location_id = b.location_id 
                              left join SD_Type c on a.type_id = c.type_id 
                              left join SD_Manufacturer d on a.manufacturer_id = d.manufacturer_id 
                              left join SD_Supplier e on a.supplier_id = e.supplier_id
                           ";
            if (!string.IsNullOrWhiteSpace(bom_code))
            {
                query += " where f.bom_code like '%' + @bom_code + '%'";

            }
            var list = _dapper.Context.Query<Bom_Input>(query, new { bom_code });
            return Ok(list);
        }

        [HttpPost("add-update")]
        [AllowAnonymous]
        public async Task<IActionResult> AddOrUpdate([FromBody] Bom_Input input)
        {

            if (input.bom_id == null)
            {
                //add
                input.bom_code = _dapper.Context.QueryFirst<string>("GetNextBomCode", commandType: CommandType.StoredProcedure);
                await _dapper.Context.ExecuteAsync (@"
                                                    INSERT INTO [dbo].[SD_Bom]
                                                               ([equipment_id]
                                                               ,[bom_code]
                                                               ,[created_at]
                                                               ,[created_by])
                                                         VALUES
                                                               (@equipment_id
                                                               ,@bom_code
                                                               ,getdate()
                                                               ,@bom_created_by)
                                                    ", input);
            } else
            {
                await _dapper.Context.ExecuteAsync(@"
                                                    update [dbo].[SD_Bom]
                                                               set equipment_id=@equipment_id
                                                               ,created_by=@bom_created_by                                                            
                                                    where bom_id=@bom_id
                                                    ", input);
            }       
            return Ok();
        }

        [HttpPost("delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Bom_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[SD_Bom]
                                                    where bom_id=@bom_id                                                     
                                                    ", input);
            return Ok();
        }
    }
}
