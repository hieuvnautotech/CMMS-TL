using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Services.OnlineUser
{
    [Table("sys_online_user")]
    public class OnlineUser 
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; }

       
        public long UserId { get; set; }
 
        [Required, MaxLength(20)]
        public string Account { get; set; }
       
        [MaxLength(20)]
        public string Name { get; set; }
 
        public DateTime LastTime { get; set; }

        [MaxLength(20)]
        public string LastLoginIp { get; set; }

        public bool? IsAccount_Locked_By_Ip { get; set; }
        public bool? Is_Account_Locked { get; set; }
    }
}
