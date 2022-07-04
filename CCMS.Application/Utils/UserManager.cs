using CCMS.Application.Core;
using CCMS.Application.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Utils
{
    public static class UserManager
    {
         
        public static long UserId => long.Parse(App.User.FindFirst(ClaimConst.CLAINM_USERID)?.Value);

       
        public static string Account => App.User.FindFirst(ClaimConst.CLAINM_ACCOUNT)?.Value;

         
        public static string Name => App.User.FindFirst(ClaimConst.CLAINM_NAME)?.Value;


        public static string Roles => App.User.FindFirst(ClaimConst.CLAINM_ROLES)?.Value;

        public static bool IsSuperAdmin => App.User.FindFirst(ClaimConst.CLAINM_SUPERADMIN)?.Value == ((int)AdminType.SuperAdmin).ToString();

      
        
    }
}
