using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{

  public class Issue_Input
    {
        public int? issue_id { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập Issue Name")]
        public string issue_name { get; set; }
        public string description { get; set; }
        public DateTime? created_at { get; set; }
        public bool? use { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Type")]
        public int type_id { get; set; }
       
      
    }
    public class Manufacturer_Input
    {
        public int? manufacturer_id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Manufacturer Name")]
        public string manufacturer_name { get; set; }
        public bool? use { get; set; }

    } 
    public class Part_Input
    {
        public int part_id { get; set; }
        public int ? id { get; set; }
        public string part_code { get; set; }
        [Required(ErrorMessage = "Vui long nhap Part Name")]
        public string part_name { get; set; }
        
        public bool use { get; set; }
        [Required(ErrorMessage ="Vui long nhap Price ")]
        public int? price { get; set; }
        public DateTime? created_at { get; set; }
        [Required(ErrorMessage ="Vui long chon Type")]
        public int? type_id { get; set; }
        [Required(ErrorMessage = "Vui long chon Manufacturer")]
        public int? manufacturer_id { get; set; }
        public int? location_id { get; set; }
        public string location_name { get; set; }
        public int? spec { get; set; }
        public string unit { get; set; }
        public string remark { get; set; }
    }
}
