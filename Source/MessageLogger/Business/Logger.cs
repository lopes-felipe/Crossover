using MessageLogger.Data;
using MessageLogger.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Business
{
    public class Logger
    {
        public Logger(DataAccess<Log> logDataAccess)
        {
            this.logDataAccess = logDataAccess;
        }

        private DataAccess<Log> logDataAccess = null;

        public bool Log(string applicationID, string logger, string level, string message)
        {
            Log log = new Log(0, applicationID, logger, level, message);
            log = this.logDataAccess.Create(log);

            // The success of the operation depends on whether the LogID was generated
            return (log.LogID > 0);
        }
    }
}
