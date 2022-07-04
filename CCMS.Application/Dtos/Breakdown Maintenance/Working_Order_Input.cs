using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.Breakdown_Maintenance
{
    public class Working_Order_Input
    {
        public int? working_order_id { get; set; }
        public string? equipment_name { get; set; }

        public int? equipment_id { get; set; }

        public string? status_name { get; set; }

        public int? status_id { get; set; }


        public string? priority_name { get; set; }

        public int? priority_id { get; set; }
        [Required(ErrorMessage = "bat buoc nhap")]
        public string? work_order { get; set; }

        public string? description { get; set; }

        public string? requestor { get; set; }

        public string? name { get; set; }

        public DateTime? due_date { get; set; }

        public DateTime? working_date { get; set; }

        public int? type_id { get; set; }

        public int? location_id { get; set; }

        public string? file { get; set; }

        public string? worker { get; set; }

        public string? type { get; set; }

        public string? location { get; set; }

        public string? remark { get; set; }

    }
}
