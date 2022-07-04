using CCMS.Application.Dtos;
using CCMS.Application.Dtos.StandardDB;
using CCMS.Application.Enum;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Api
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings("WMS Asset", Tag = "WMS Asset Stock", Order = 300)]
    public class StockApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;

        public StockApiController(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll(string equipment_code,string location_name, string type_name, string start_date_search, string end_date_search)
        {
            var query = @"select 
                          a.*, 
                          b.equipment_name, 
                          d.location_name, 
                          c.type_name 
                        from 
                          TT_Equipment_Receiving a 
                          left join SD_Equipment b on a.equipment_code = b.equipment_code 
                          left join SD_Type c on c.type_id = b.type_id 
                          left join SD_Location d on d.location_id = b.location_id 
                        where a.status='INSTOCK'
                        ";
            if (!string.IsNullOrWhiteSpace(equipment_code))
            {
                query += " and a.equipment_code like '%' + @equipment_code + '%'";

            }
            if (!string.IsNullOrWhiteSpace(location_name))
            {
                query += " and d.location_name like '%' + @location_name + '%'";

            }
            if (!string.IsNullOrWhiteSpace(type_name))
            {
                query += " and c.type_name like '%' + @type_name + '%'";

            }
            if (!string.IsNullOrWhiteSpace(start_date_search))
            {
                query += " and @start_date_search <= a.receiving_date";

            }
            if (!string.IsNullOrWhiteSpace(end_date_search))
            {
                query += " and a.receiving_date <= @end_date_search";

            }
            var list = _dapper.Context.Query<Receiving_Input>(query,new { equipment_code, location_name, type_name, start_date_search, end_date_search });
            return Ok(list);
        }         
    }
}
