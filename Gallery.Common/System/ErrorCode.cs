using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.System
{
    public static class ErrorCode
    {
        //See for error codes. https://msdn.microsoft.com/en-us/library/windows/desktop/ms681382(v=vs.85).aspx
        public const int ERROR_SHARING_VIOLATION = 0x00000020;
        public const int ERROR_ALREADY_EXISTS = 0x000000B7;

        public static int GetFromException(Exception exception)
        {
            return exception.HResult & 0xFFFF;
        }
    }
}
