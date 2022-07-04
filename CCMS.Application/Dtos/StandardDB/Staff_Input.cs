using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class Staff_Input
    {
        public int? staff_id { get; set; }
        [Required(ErrorMessage = "bat buoc nhap")]
        public string staff_name { get; set; }
        public DateTime? created_at { get; set; }
        public string? email { get; set; }
        public string? remark { get; set; }
        public string? call { get; set; }
        public int? department_id { get; set; }
        public string? department_name { get; set; }
    }
}
