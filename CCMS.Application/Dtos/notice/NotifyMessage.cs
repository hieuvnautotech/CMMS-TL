using CCMS.Application.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.notice
{
    public class NotifyMessage
    {
        public MessageType type { get; set; }
        public string typestring
        {
            get
            {
                if (type == MessageType.notifyupload) return "notifyupload";
                else if (type == MessageType.lock_account) return "lock_account";
                else if (type == MessageType.force_exit) return "force_exit";
                else if (type == MessageType.alert_message) return "alert_message";

                return "";
            }
        }
        public dynamic data { get; set; }
    }
}
