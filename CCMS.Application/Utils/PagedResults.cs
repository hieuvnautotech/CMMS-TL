using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Utils
{
    public class PagedResults<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
