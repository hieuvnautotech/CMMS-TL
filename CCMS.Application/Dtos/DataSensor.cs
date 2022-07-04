using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class DataSensor
    {
        [CsvHelper.Configuration.Attributes.IndexAttribute(1)]
        public decimal sensor_0 { get; set; }

        [CsvHelper.Configuration.Attributes.IndexAttribute(2)]
        public decimal sensor_1 { get; set; }

        [CsvHelper.Configuration.Attributes.IndexAttribute(3)]
        public decimal sensor_2 { get; set; }

        [CsvHelper.Configuration.Attributes.IndexAttribute(4)]
        public decimal sensor_3 { get; set; }

        [CsvHelper.Configuration.Attributes.IndexAttribute(5)]
        public decimal sensor_4 { get; set; }
        [CsvHelper.Configuration.Attributes.IndexAttribute(6)]
        public decimal sensor_5 { get; set; }
        [CsvHelper.Configuration.Attributes.IndexAttribute(7)]
        public decimal sensor_6 { get; set; }
        [CsvHelper.Configuration.Attributes.IndexAttribute(8)]
        public decimal sensor_7 { get; set; }


    }
}
