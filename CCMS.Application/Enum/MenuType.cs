using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Enum
{
    public enum MenuType
    {
        /// <summary>
        /// Folder
        /// </summary>
        [Description("Folder")]
        DIR = 0,

        /// <summary>
        /// menu
        /// </summary>
        [Description("menu")]
        MENU = 1,

        /// <summary>
        /// Button
        /// </summary>
        [Description("Button")]
        BTN = 2
    }

    public enum MenuLocation
    {
  
        [Description("StandardDB")]
        STANDARDDB = 0,

        [Description("WMS")]
        WMS = 1,

     
    }

}
