using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.notice
{
    public class NoticeBase
    {

        public string Title { get; set; }


        public string Content { get; set; }

        public int Type { get; set; }


        public long PublicUserId { get; set; }


        public string PublicUserName { get; set; }


        public long PublicOrgId { get; set; }

        public string PublicOrgName { get; set; }


        public DateTime? PublicTime { get; set; }


        public DateTime? CancelTime { get; set; }


        public int Status { get; set; }
    }
}
