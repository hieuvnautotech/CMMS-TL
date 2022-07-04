using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Enum
{
    
        [ErrorCodeType]
        public enum ErrorCode
        {
       

        /// <summary>
        /// Incorrect username or password
        /// </summary>
        [ErrorCodeItemMetadata("username or password không đúng")]
            D1000,

            /// <summary>
            /// Illegal operation! Forbid to delete yourself
            /// </summary>
            [ErrorCodeItemMetadata("Illegal operation, it is forbidden to delete yourself")]
            D1001,

            /// <summary>
            /// The record does not exist
            /// </summary>
            [ErrorCodeItemMetadata("Record không tồn tại")]
            D1002,

            /// <summary>
            /// Account already exists
            /// </summary>
            [ErrorCodeItemMetadata("Account đã tồn tại")]
            D1003,

            /// <summary>
            /// old password does not match
            /// </summary>
            [ErrorCodeItemMetadata("The old password was entered incorrectly")]
            D1004,

        [ErrorCodeItemMetadata("Your account is locked !!")]
        D1005,
        [ErrorCodeItemMetadata("Your account with this Ip is locked !!")]
        D1006,


        /// </summary>
        [ErrorCodeItemMetadata("Menu already exists")]
        D4000,

        /// <summary>
        /// The routing address is empty
        /// </summary>
        [ErrorCodeItemMetadata("Route address is empty")]
        D4001,

        /// <summary>
        /// The open method is empty
        /// </summary>
        [ErrorCodeItemMetadata("Open method is empty")]
        D4002,

        /// <summary>
        /// The permission ID format is empty
        /// </summary>
        [ErrorCodeItemMetadata("Permission ID format is empty")]
        D4003,

        /// <summary>
        /// Permission ID format error
        /// </summary>
        [ErrorCodeItemMetadata("Permission ID format error")]
        D4004,

        /// <summary>
        /// Permission does not exist
        /// </summary>
        [ErrorCodeItemMetadata("Permission does not exist")]
        D4005,


        [ErrorCodeItemMetadata("The parent menu cannot be the current node, please re-select the parent menu")]
        D4006,

        /// <summary>
        /// The root node cannot be moved
        /// </summary>
        [ErrorCodeItemMetadata("Cannot move the root node")]
        D4007,

        /// <summary>
        /// The root node cannot be moved
        /// </summary>
        [ErrorCodeItemMetadata("Menu Id is not exists")]
        D4008,


        [ErrorCodeItemMetadata("Parent Menu is not  exists")]
        D4009,




        //nhat
        [ErrorCodeItemMetadata("Code had")]
        N1001,

        [ErrorCodeItemMetadata("Code error")]
        N1002,

        [ErrorCodeItemMetadata("Nhập code")]
        N1003,

        //toa2n
        [ErrorCodeItemMetadata("Tên manufacturer này đã có trong hệ thống")]
        T001,


        [ErrorCodeItemMetadata("Tên issue này đã có trong hệ thống")]
        T002,

        /// <summary>
        /// Notification announcement status error
        /// </summary>
        [ErrorCodeItemMetadata("Notification announcement status error")]
        D7000,

        /// <summary>
        /// Notify that the announcement failed to be deleted
        /// </summary>
        [ErrorCodeItemMetadata("Notification announcement deletion failed")]
        D7001,

        /// <summary>
        /// Notify announcement edit failed
        /// </summary>
        [ErrorCodeItemMetadata("Notification announcement editing failed, the type must be draft")]
        D7002,
    }
    
}
