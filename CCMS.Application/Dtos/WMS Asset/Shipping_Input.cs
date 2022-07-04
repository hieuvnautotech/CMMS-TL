using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.StandardDB
{
    public class Shipping_Input
    {
        public int? shipping_id { get; set; }


        public int? equipment_id { get; set; }

         
        public string equipment_code { get; set; }

        
        public DateTime? shipping_date { get; set; }

        
        public string status { get; set; }

          
        public string created_by { get; set; }

         
        public DateTime? created_at { get; set; }

        public string location_name { get; set; }
        public string type_name { get; set; }

        public string equipment_name { get; set; }
        public string create_at_fomat
        {
            get
            {
                if (created_at == null) return null;
                else return created_at.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public string shipping_date_fomat
        {
            get
            {
                if (shipping_date == null) return null;
                else return shipping_date.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }


    }
}
