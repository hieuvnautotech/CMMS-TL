using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    internal class Type_Model_GetLit
    {
       
        public int? type_id { get; set; }
        public int? id => type_id;


        public string type_name { get; set; }

        public bool? use { get; set; }  


        public string use_string
        {
            get {
                if (use == true) return "Y";
                else return "N";

            }
        }
    }
}
