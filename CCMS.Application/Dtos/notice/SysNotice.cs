using CCMS.Application.Enum;
using CCMS.Application.Services;
using CCMS.Application.Services.Notice;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.notice
{
    public class SysNotice
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public NoticeType Type { get; set; }
        public long PublicUserId { get; set; }
        public string PublicUserName { get; set; }
        public int PublicOrgId { get; set; }
        public string PublicOrgName { get; set; }

        public DateTime? CreatedTime { get; set; }
        public string CreatedUserName { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime? PublicTime { get; set; }

        public DateTime? CancelTime { get; set; }
        public NoticeStatus Status { get; set; }
    }
}
