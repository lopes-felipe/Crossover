using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Exceptions
{
    public class UnexpectedException
        : InternalException
    {
        public UnexpectedException(int errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        {

        }

        public UnexpectedException(int errorCode, string errorMessage, Exception innerException)
            : base(errorCode, errorMessage, innerException)
        {

        }
    }
}
