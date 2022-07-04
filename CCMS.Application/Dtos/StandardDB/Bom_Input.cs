using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.StandardDB
{
    public class Bom_Input: Equipment_Input
    {
        public string bom_code { get; set; }

        public int? equipment_id { get; set; }

      
        public int? bom_id { get; set; }

        
        public string? bom_created_by { get; set; }

        public DateTime? bom_created_at { get; set; }  
    }
}
