using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class SupplierModel_Input
    {
        public int? supplier_id { get; set; }

        [Required(ErrorMessage = "bat buoc nhap")]
        public string supplier_name { get; set; }

        

        
    }
    
    

}
