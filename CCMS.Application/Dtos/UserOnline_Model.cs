using CCMS.Application.Dtos.notice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class UserOnline_Model
    {
        [Required(ErrorMessage ="UserId khong de trong")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Message khong de trong")]
        public MessageInfo  Message { get; set; }

        public string Ip { get; set; }

        
    }
}
