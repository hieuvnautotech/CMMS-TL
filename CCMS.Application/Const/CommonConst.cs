using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Const
{
    public class CommonConst
    {
        public const string CACHE_KEY_USER = "user_";

       
        public const string CACHE_KEY_MENU = "menu_";

       
        public const string CACHE_KEY_PERMISSION = "permission_";

       
        public const string CACHE_KEY_DATASCOPE = "datascope_";
        public const string CACHE_KEY_USERSDATASCOPE = "usersdatascope_";

        
        public const string CACHE_KEY_CODE = "vercode_";

      
        public const string CACHE_KEY_ENTITYINFO = "tableentity";

        
        public const string CACHE_KEY_ALLPERMISSION = "allpermission";
 

        
        public static string[] ENTITY_ASSEMBLY_NAME = new string[] { "Magic.Core", "Magic.Application", "Magic.FlowCenter" };
        
        public const string DELETE_FIELD = "IsDeleted";
    }
}
