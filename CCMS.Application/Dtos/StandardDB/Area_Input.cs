using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class Area_Input
    {
        [Required(ErrorMessage = "bat buoc nhap")]
        public string area_name { get; set; }

        public string remark { get; set; }

        public int? area_id { get; set; }

        public int? type_id { get; set; }

    }
}
