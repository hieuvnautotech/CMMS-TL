using CCMS.Application.Dtos.EquipmentManager;
using CCMS.Application.Services;
using CCMS.Core.Filter;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Api
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings("Equipment Manager@1",Tag = "Quản lý Equipment")]

    public class EquipmentManagerApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;
        private readonly IEquipmentManagerService _equipManager;

        public EquipmentManagerApiController(IDapperRepository dapper, IEquipmentManagerService equipManager)
        {
            _dapper = dapper;
            _equipManager = equipManager;

        }

        [HttpPost("get-single-part")]
        public IActionResult GetSinglePart(  [FromBody] Part_Model input)
        {
            return Ok(_equipManager.GetSinglePart(input.part_id));
        }

        [HttpGet("get-parts")]
        public IActionResult GetParts()
        {
            return Ok(_equipManager.GetTreeParts());
        }
    }
}
