using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Exceptions
{
    public class BackendInfrastructureException
        : InternalException
    {
        public BackendInfrastructureException(int errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        {

        }

        public BackendInfrastructureException(int errorCode, string errorMessage, Exception innerException)
            : base(errorCode, errorMessage, innerException)
        {

        }
    }
}
