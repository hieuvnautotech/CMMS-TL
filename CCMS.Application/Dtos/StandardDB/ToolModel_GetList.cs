using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.StandardDB
{
    public class ToolModel_GetList
    {
        public int tool_id { get; set; }
        public int id => tool_id;
       
        public string tool_name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Spec")]
        public string spec { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Type")]
        public int type_id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Price")]
        public decimal price { get; set; }
 
        public string use { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Unit")]
        public string unit { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Manufacturer")]
        public int manufacturer_id { get; set; }
       
    
        public DateTime? created_at { get; set; }
        public string created_at_format
        {

            get
            {
                if (created_at == null) return null;

                return created_at.Value.ToString("yyyy-MM-dd hh:mm:s");
            }
        }
        //ngoại biến
        public string manufacturer_name { get; set; }
        public string type_name { get; set; }

    }
}
