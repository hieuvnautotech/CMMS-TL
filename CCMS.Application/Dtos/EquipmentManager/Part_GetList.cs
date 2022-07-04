using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.EquipmentManager
{
    public class Part_Model_Single :Part_Model
    {
        public string location_name { get; set; }
        public string type_name { get; set; }
        public string manufacturer_name { get; set; }
        public string parent_name { get; set; }

    }
    public class Part_Model
    {
        public int part_id { get; set; }

        public string part_code { get; set; }

        public string part_name { get; set; }

        public string spec { get; set; }

        public int? type_id { get; set; }

        public int? location_id { get; set; }

        public decimal? price { get; set; }

        public string unit { get; set; }

        public string appropriate_qty { get; set; }

        public bool? use { get; set; }

        public int? manufacturer_id { get; set; }

        public string remark { get; set; }

        public string image_url { get; set; }

        public string created_by { get; set; }

        public DateTime? created_at { get; set; }

        public int? parent_id { get; set; }


    }
}
