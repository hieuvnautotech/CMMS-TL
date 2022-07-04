using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.System
{
    public class sysRoleModel
    {
        public int? id { get; set; }
        [Required(ErrorMessage = "Required name")]
        public string name { get; set; }
        public string remark { get; set; }
        public bool? active { get; set; }
    }
}
