using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Enum
{
    public enum NoticeStatus
    {

        [Description("draft")]
        DRAFT = 0,


        [Description("release")]
        PUBLIC = 1,


        [Description("withdraw")]
        CANCEL = 2,

        [Description("delete")]
        DELETED = 3
    }
}
