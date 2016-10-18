using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Exceptions
{
    public class AuthenticationException
        : UnauthorizedException
    {
        public AuthenticationException(int errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        {

        }

        public AuthenticationException(int errorCode, string errorMessage, Exception innerException)
            : base(errorCode, errorMessage, innerException)
        {

        }
    }
}
