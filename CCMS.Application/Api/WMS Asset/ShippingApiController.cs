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
    [ApiDescriptionSettings("WMS Asset", Tag = "WMS Asset Shipping", Order = 101)]
    public class ShippingApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;

        public ShippingApiController(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllShipping(string search_name)
        {
            var query = @"select 
                          a.*, 
                          b.equipment_name, 
                          d.location_name, 
                          c.type_name 
                        from 
                          TT_Equipment_Shipping a 
                          left join SD_Equipment b on a.equipment_code = b.equipment_code 
                          left join SD_Type c on c.type_id = b.type_id 
                          left join SD_Location d on d.location_id = b.location_id 
                        where 1=1
                        ";
            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += " and a.equipment_code like '%' + @search_name + '%'";

            }
            query += @"order by 
                       a.shipping_id desc";
            var list = _dapper.Context.Query<Shipping_Input>(query,new { search_name });
            return Ok(list);
        }

        [HttpPost("scan")]
        [AllowAnonymous]
        public async Task<IActionResult> Scan([FromBody] Shipping_Input input)
        {

                var equipment_code = input.equipment_code;

            if (equipment_code == "")
            {
                throw Oops.Oh(ErrorCode.N1003);
            }
            else 
            {
                var obj = _dapper.Context.QueryFirstOrDefault<dynamic>(@"select * from SD_Equipment a where a.equipment_code = @equipment_code", new { equipment_code });

                if (obj != null)
                {
                    var obj1 = _dapper.Context.QueryFirstOrDefault<dynamic>(@"select * from TT_Equipment_Shipping a where a.equipment_code = @equipment_code", new { equipment_code });
                    if (obj1 == null)
                    {
                        //var obj2 = new Dictionary<string, object>(obj1);
                        //obj2.Add("datatime", DateTime.Now);

                        await _dapper.Context.ExecuteAsync(@"
                                                    INSERT INTO [dbo].[TT_Equipment_Shipping]
                                                               ([equipment_code]
                                                               ,[equipment_id]
                                                               ,[shipping_date]
                                                               ,[created_at]
                                                               ,[status])
                                                         VALUES
                                                               (@equipment_code
                                                               ,@equipment_id
                                                               ,getdate()
                                                               ,getdate()
                                                               ,'RECEIVING')
                                                    ", new Dictionary<string, object>(obj));
                    }
                    else
                    {
                        throw Oops.Oh(ErrorCode.N1001);
                    }
                }
                else
                {
                    throw Oops.Oh(ErrorCode.N1002);
                }
            }

                
                return Ok();
            


        }

        [HttpPost("delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] Shipping_Input input)
        {


            await _dapper.Context.ExecuteAsync(@"
                                                    delete from [dbo].[TT_Equipment_Shipping]
                                                    where shipping_id=@shipping_id                                                   
                                                    ", input);
            return Ok();
        }
    }
}
