using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.notice
{
    public class DeleteNoticeInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Notice announced ID cannot be empty")]
        public long Id { get; set; }
    }
}
