using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Entities
{
    public class Log
    {
        public Log()
        {

        }

        public Log(int logID)
            : this()
        {
            this.LogID = logID;
        }

        public Log(int logID, string applicationID, string logger, string level, string message)
            : this(logID)
        {
            this.ApplicationID = applicationID;
            this.Logger = logger;
            this.Level = level;
            this.Message = message;
        }

        public int LogID { get; set; }

        public string ApplicationID { get; set; }

        public string Logger { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }
    }
}
