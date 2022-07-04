using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.StandardDB
{
    public class Issue_Model_GetLit
    {
        public int? issue_id { get; set; }
        public int? id => issue_id;
        public string type_name { get; set; }   
  

        public string issue_name { get; set; }
         public string description { get; set; }
        public int ? type_id { get; set; }
        public bool? use { get; set; }

        public string use_string
        {
            get
            {
                if (use == true) return "Y";
                else return "N";

            }
        }
        public DateTime? created_at { get; set; }


        public string created_at_format
        {

            get
            {
                if (created_at == null) return null;

                return created_at.Value.ToString("yyyy-MM-dd");
            }
        }
    }
    public class Part_Model_GetList
    {
        public int? part_id { get; set; }
        public int? id { get; set; }
        public string part_code { get; set; }
        public string part_name { get; set; }
        public int price { get; set; }
        public string price2 { get; set; }
        public bool use { get; set; }
        public int? location_id { get; set; }
        public string unit { get; set; }    
        public string remark { get; set; }
        public DateTime? created_at { get; set; }
        public string type_name{ get; set; }
        public string location_name { get; set; }
        public string manufacturer_name { get; set; }
        public int? manufacturer_id { get; set; }
        public int type_id { get; set; }
        public int spec { get; set; }
        public string use_string
        {
            get { return use == true ? "Y" : "N"; }
        }
        public string create_at_fomat
        {
            get
            {
                if (created_at == null) return null;
                else return created_at.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
