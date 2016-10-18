using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Exceptions
{
    public class ForbiddenException
        : BaseException
    {
        public ForbiddenException(int errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        {

        }

        public ForbiddenException(int errorCode, string errorMessage, Exception innerException)
            : base(errorCode, errorMessage, innerException)
        {

        }
    }
}
