using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.StandardDB
{
    internal class Manufacturer_Model_GetLit
    {
        public int? manufacturer_id { get; set; }
        public int? id => manufacturer_id;


        public string manufacturer_name { get; set; }

        public bool? use { get; set; }


        public string use_string
        {
            get
            {
                if (use == true) return "Y";
                else return "N";

            }
        }
    }
}
