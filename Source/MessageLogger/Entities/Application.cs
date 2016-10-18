using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Entities
{
    public class Application
    {
        public Application()
        {

        }

        public Application(string applicationID)
            : this()
        {
            this.ApplicationID = applicationID;
        }

        public Application(string applicationID, string displayName, string secret)
            : this(applicationID)
        {
            this.DisplayName = displayName;
            this.Secret = secret;
        }

        public Application(string applicationID, string displayName, string secret, DateTime restrictedAccessUntil)
            : this(applicationID, displayName, secret)
        {
            this.RestrictedAccessUntil = restrictedAccessUntil;
        }

        public string ApplicationID { get; set; }

        public string DisplayName { get; set; }

        public string Secret { get; set; }

        public DateTime RestrictedAccessUntil { get; set; }
    }
}
