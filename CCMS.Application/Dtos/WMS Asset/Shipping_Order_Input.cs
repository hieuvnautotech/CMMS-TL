using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.StandardDB
{
    public class Shipping_Master
    {
        public int? shipping_id { get; set; }

        [Required(ErrorMessage ="name bắt  buộc nhập")]
        public string shipping_name { get; set; }

        public string so { get; set; }
        public DateTime? etd { get; set; }
        public int? location_id { get; set; }


        public string status { get; set; }


        public DateTime? reg_date { get; set; }


        public string remark { get; set; }

        public string created_by { get; set; }

        public string location_name { get; set; }

        public string etd_fomat
        {
            get
            {
                if (etd == null) return null;
                else return etd.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public string reg_date_fomat
        {
            get
            {
                if (reg_date == null) return null;
                else return reg_date.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

    }
    public class Shipping_Detail
    {
        public int? shipping_order_id { get; set; }

        public string name { get; set; }

        public int? shipping_id { get; set; }

        public int? equipment_id { get; set; }

        public int? part_id { get; set; }

        public int? location_id { get; set; }

        public string status { get; set; }

        public int so_qty { get; set; }

        public int? unit_id { get; set; }


        public int? shipped_qty { get; set; }

        public string created_by { get; set; }

        public DateTime? created_at { get; set; }

        public string? equipment_code { get; set; }
        public string? location_name { get; set; }
        public string? unit_name { get; set; }

        public string part_name { get; set; }

        public string shipping_name { get; set; }
        public string created_at_fomat
        {
            get
            {
                if (created_at == null) return null;
                else return created_at.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

    }
}
