using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.StandardDB
{
    public class Unit_Input
    {
        public string manufacturer_name { get; set; }

        public int? unit_id { get; set; }

      
        public string unit_name { get; set; }

        
        public string unit_remark { get; set; }

      
        public bool? use { get; set; }
      
        public int? manufacturer_id { get; set; }
    }
}
