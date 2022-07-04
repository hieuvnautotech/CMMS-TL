using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Enum
{
    public enum MessageType
    {
         
        [Description("thông báo khi có file đã được upload trong hàm upload Dashboard1Api")]
        notifyupload,

        
       
        force_exit,

        alert_message,
        lock_account



    }
}
