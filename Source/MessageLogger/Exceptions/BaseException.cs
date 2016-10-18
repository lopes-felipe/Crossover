using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Exceptions
{
    public class BaseException
        : Exception
    {
        public BaseException(int errorCode, string errorMessage)
            : this(errorCode, errorMessage, null)
        {

        }

        public BaseException(int errorCode, string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
            this.ErrorCode = errorCode;
        }

        public int ErrorCode { get; set; }
    }
}
