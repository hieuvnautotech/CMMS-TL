using Furion.ConfigurableOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Options
{
    public class AppDatabaseConfig: IConfigurableOptions
    {
        public string sqlite { get; set; }
        public string mssql { get; set; }
        public int active { get; set; }

        public string GetConnectinStringActive()
        {
            if (active == 0) return   sqlite; else return    mssql;
        }

    }
}
