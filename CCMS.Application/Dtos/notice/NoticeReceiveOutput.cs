using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.notice
{
    public class NoticeReceiveOutput : NoticeBase
    {

        public long Id { get; set; }


        public int ReadStatus { get; set; }

        public DateTime? ReadTime { get; set; }
    }
}
