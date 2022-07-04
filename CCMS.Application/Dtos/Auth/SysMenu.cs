using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.Auth
{
    [Table("sys_menu")]
    public class SysMenu 
    {
       public long Id { get; set; } 
        [Required, MaxLength(20)]
        public string Name { get; set; }

       
        [Required, MaxLength(50)]
        public string Code { get; set; }

        
        public int Type { get; set; }

        
        [MaxLength(20)]
        public string Icon { get; set; }

       
        [MaxLength(100)]
        public string Router { get; set; }

        
        [MaxLength(100)]
        public string Component { get; set; }

         
        [MaxLength(100)]
        public string Permission { get; set; }

        
    
        public bool? Visible { get; set; }


        public int Sort { get; set; } = 100;

        public long? Pid { get; set; }
        public string Pids { get; set; }

        public bool? IsShowDefault { get; set; }

        public string Title { get; set; }



    }
}
