using CCMS.Application.Enum;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.Auth
{
    [Table("sys_log_vis")]
    public class sys_log_vis 
    {
        public  int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
 
        public YesOrNot Success { get; set; }

      
        public string Message { get; set; }

     
        [MaxLength(20)]
        public string Ip { get; set; }

       
        [MaxLength(100)]
        public string Location { get; set; }
 
        [MaxLength(100)]
        public string Browser { get; set; }

      
        [MaxLength(100)]
        public string Os { get; set; }

       
       
        
        public DateTime VisTime { get; set; }

        
        [MaxLength(20)]
        public string Account { get; set; }
    }
}
