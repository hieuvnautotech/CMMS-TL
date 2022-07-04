using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class SupplierModel_GetList
    {
        public int? supplier_id { get; set; }
        public int? id => supplier_id;

        public string supplier_name { get; set; }

        public DateTime? created_at { get; set; }


        public string created_at_format  {

            get
            {
                if (created_at == null) return null;

                return   created_at.Value.ToString("yyyy-MM-dd hh:mm:s");
            }

            }

    }
}
