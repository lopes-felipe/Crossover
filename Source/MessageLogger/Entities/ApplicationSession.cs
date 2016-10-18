using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Entities
{
    public class ApplicationSession
    {
        public ApplicationSession()
        {

        }

        public ApplicationSession(long sessionID)
            : this()
        {
            this.SessionID = sessionID;
        }

        public ApplicationSession(long sessionID, string applicationID, Guid accessToken, bool? active, DateTime createdDate, DateTime validUntil)
            : this(sessionID)
        {
            this.ApplicationID = applicationID;
            this.AccessToken = accessToken;
            this.Active = active;
            this.CreatedDate = createdDate;
            this.ValidUntil = validUntil;
        }

        public long SessionID { get; set; }

        public string ApplicationID { get; set; }

        public Guid AccessToken { get; set; }

        public bool? Active { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ValidUntil { get; set; }
    }
}
