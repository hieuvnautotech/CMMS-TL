using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.StandardDB
{
    public class Machine_Input
    {
        public int? machine_id { get; set; }

        public int? area_id { get; set; }

        public string? area_name { get; set; }

        public int? supplier_id { get; set; }

        public string? supplier_name { get; set; }

        public int? manufacturer_id { get; set; }
        public string? manufacturer_name { get; set; }

        public int? price { get; set; }

        public int? serial_number { get; set; }

        public string? remark { get; set; }

        public DateTime? made_date { get; set; }

        public DateTime? install_date { get; set; }

        public bool? use { get; set; }

        public int? type_id { get; set; }

        public string? type_name { get; set; }
        [Required(ErrorMessage = "bat buoc nhap")]
        public string machine_name { get; set; }

        public int? status_id { get; set; }

        public string? status_name { get; set; }

        public int? unit_id { get; set; }

        public string? unit_name { get; set; }

        public int? location_id { get; set; }

        public string? location_name { get; set; }

        public string? machine_code { get; set; }

        public string made_date_format
        {

            get
            {
                if (made_date == null) return null;

                return made_date.Value.ToString("yyyy-MM-dd");
            }
        }
    }
}
