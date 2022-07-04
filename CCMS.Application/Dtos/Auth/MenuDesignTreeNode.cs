using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.Auth
{
    public class MenuDesignTreeNode
    {
        
       
        public long? Id { get; set; }


        public long? Pid { get; set; }

        public string Name { get; set; }

       
        public string Component { get; set; }


        public string Router { get; set; }
        public Meta Meta { get; set; }

        public bool? IsShowDefault { get; set; }
         public bool? Visiable { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }


    }

    public class Meta
    {
        
        public string Title { get; set; }

       
        public string Icon { get; set; }

        
        public bool Show { get; set; }

        public string Target { get; set; }


        public string Router { get; set; }
    }
}
