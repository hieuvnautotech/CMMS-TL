using CCMS.Application.Enum;
using CCMS.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.notice
{
    public class NoticeInput : PageInputBase
    {

        public virtual string Title { get; set; }


        public virtual string Content { get; set; }


        public virtual NoticeType Type { get; set; }


        public virtual NoticeStatus Status { get; set; }


        public virtual List<long> NoticeUserIdList { get; set; }
    }

}
