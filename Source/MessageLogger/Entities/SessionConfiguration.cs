using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Entities
{
    public class SessionConfiguration
    {
        public SessionConfiguration()
        {

        }

        public SessionConfiguration(int configurationID)
            : this()
        {
            this.ConfigurationID = configurationID;
        }

        public SessionConfiguration(int configurationID, int sessionLifetimeMinutes)
            : this(configurationID)
        {
            this.SessionLifetimeMinutes = sessionLifetimeMinutes;
        }

        public int ConfigurationID { get; set; }

        public int SessionLifetimeMinutes { get; set; }
    }
}
