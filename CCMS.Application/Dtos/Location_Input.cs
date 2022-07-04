using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class Location_Input
    {
        public int? location_id { get; set; }
        [Required(ErrorMessage = " location code ")]
        public string location_code { get; set; }
        [Required(ErrorMessage = "location name bắt buộc nhập")]
        public string location_name { get; set; }
        public string remark { get; set; }

        public bool? use { get; set; }

        public int? type_id { get; set; }

    }
    public class Type_Input
    {
        public int? type_id { get; set; }

        [Required(ErrorMessage ="Bắt buộc nhập")]
        public string type_name { get; set; }
        public bool? use { get; set; }
    }

}
