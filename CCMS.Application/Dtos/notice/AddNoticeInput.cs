using CCMS.Application.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.notice
{
    public class AddNoticeInput : NoticeInput
    {

        [Required(ErrorMessage = "The title can not be blank")]
        public override string Title { get; set; }


        [Required(ErrorMessage = "the content can not be blank")]
        public override string Content { get; set; }


        [Required(ErrorMessage = "Type cannot be empty")]
        public override NoticeType Type { get; set; }

        /// <summary>
        /// Status (dictionary 0 draft 1 published 2 withdrawn 3 deleted)
        /// </summary>
        [Required(ErrorMessage = "The state cannot be empty")]
        public override NoticeStatus Status { get; set; }


        [Required(ErrorMessage = "The person who notified the notification cannot be empty")]
        public override List<long> NoticeUserIdList { get; set; }
    }

}
