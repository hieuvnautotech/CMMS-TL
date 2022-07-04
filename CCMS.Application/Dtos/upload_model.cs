using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class upload_model_input: upload_model
    {
        public int? id { get; set; }
    }
    public class upload_model
    {

       
        public string Location { get; set; }
        public string Machine { get; set; }
        public string Staff { get; set; }
        public string Left { get; set; }
        public string Size { get; set; }
        public string PathFile { get; set; }
        public string Status { get; set; }
        public IFormFile filetext { get; set; }

        public bool? IsRead { get; set; }
        public string image_path { get; set; }
    }
}
