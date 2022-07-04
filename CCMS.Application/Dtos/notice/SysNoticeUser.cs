using CCMS.Application.Enum;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.notice
{
    [Table("sys_notice_user")]
    public class SysNoticeUser
    {
        public long NoticeId { get; set; }


        public long UserId { get; set; }


        public DateTime? ReadTime { get; set; }


        public NoticeUserStatus ReadStatus { get; set; }
    }
}
