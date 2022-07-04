using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.Dashboard1Dtos
{
    public class Status_Sensor_Model
    {
        public string status { get; set; }
        public int CountNg { get; set; }
        public int CountOk { get; set; }
        public int CountFixed { get; set; }
        public string Created_Date { get; set; }

    }
}
