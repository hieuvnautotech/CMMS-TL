using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.StandardDB
{
    public class Parts_Key_value
    {
        public string title { get; set; }
        public int value { get; set; }   
    }
    public class Equipment_Input
    {
        public string equipment_code { get; set; }

        public int? equipment_id { get; set; }

      
        public string equipment_name { get; set; }

        
        public string status { get; set; }

      
        public string? equipment_value { get; set; }

        public string? serial_number { get; set; }

        public int? price { get; set; }

        public DateTime? made_date { get; set; }

        public DateTime? install_date { get; set; }

        public DateTime? created_at { get; set; }

        public bool? use { get; set; }

        public int? supplier_id { get; set; }

        public int? type_id { get; set; }

        public int? location_id { get; set; }

        public int? manufacturer_id { get; set; }

        public string? remark { get; set; }

        public string? created_by { get; set; }

        public string? level { get; set; }

        public string? image_url { get; set; }

        public string? supplier { get; set; }

        public string? type { get; set; }

        public string? location { get; set; }

        public string? manufacturer { get; set; }

        public List<Parts_Key_value> partCollection { get; set; }   
    }
}
