using Furion.ConfigurableOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Options
{
    public sealed class RefreshTokenSettingOptions : IConfigurableOptions
    {
       
        public int ExpiredTime { get; set; } = 43200;
    }
}
