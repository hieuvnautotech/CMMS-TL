using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Enum
{
   
    [Description("Loai user dang nhap")]
    public enum AdminType
    {
        
        [Description("root user")]
        SuperAdmin = 1,

       
        [Description("admin customer")]
        Admin = 2,

         
        [Description("regular")]
        None = 3
    }
}
