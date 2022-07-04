using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Enum
{
    public enum NoticeUserStatus
    {

        [Description("unread")]
        UNREAD = 0,

        [Description("Have read")]
        READ = 1
    }
}
