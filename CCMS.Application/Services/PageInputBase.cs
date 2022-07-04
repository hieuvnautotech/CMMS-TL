using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Services
{
    public class PageInputBase
    {
        /// <summary>
        /// search value
        /// </summary>
        public virtual string SearchValue { get; set; }

        /// <summary>
        /// Current page number
        /// </summary>
        public virtual int PageNo { get; set; } = 1;

        /// <summary>
        /// Page capacity
        /// </summary>
        public virtual int PageSize { get; set; } = 20;

        /// <summary>
        /// Search start time
        /// </summary>
        public virtual string SearchBeginTime { get; set; }

        /// <summary>
        /// Search end time
        /// </summary>
        public virtual string SearchEndTime { get; set; }


        public virtual string SortField { get; set; }

        /// <summary>
        ///Sorting method, default ascending order, otherwise descending order (with antd front-end, the agreed parameters are Ascend, Dscend)
        /// </summary>
        public virtual string SortOrder { get; set; }

        /// <summary>
        ///Sort in descending order (don't ask me why descend is not desc, the front-end convention parameters are just like this)
        /// </summary>
        public virtual string DescStr { get; set; } = "descend";
    }
}
